using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thumbsCollector.Core.Interfaces;
using thumbsCollector.Input;
using thumbsCollector.Output;
using thumbsCollector.Validations;

namespace thumbsCollector.Core
{
    public class Engine : IEngine
    {
        public void Run()
        {
            GlobalConstants gc = new GlobalConstants();
            DDebugg debug = new DDebugg();
            printAndExport printExport = new printAndExport();

            Console.WriteLine("........................");
            Console.WriteLine("....THUMBS COLLECTOR....");
            Console.WriteLine("........................");
            Console.WriteLine();

            Console.Write("SEASON :");
            Console.WriteLine();

            getInfo getSeasonalInfo = new getInfo();
            string inputSeason = getSeasonalInfo.currentSeason();

            Validator validator = new Validator(inputSeason);
            inputSeason = validator.ValidateSeason();
            string validationPattern = validator.Pattern();

            //get allfiles and used geometries
            var allFilesPsd = getSeasonalInfo
                .GetAllFilesAsync(gc.psdExtension, debug.MenWomen, debug.YoungAthletes, debug.PlusSize); //psd search
            var geometryInUse = getSeasonalInfo.GeometryInUse(allFilesPsd, validationPattern);

            //main thumbs folder
            string thumbnailsFolder = debug.thumbnailsFolder;

            //enter directory where you want to save thumbs
            Console.WriteLine();
            Console.WriteLine("Please enter directory to save thumbs:".ToUpper());
            string destinationPath = getSeasonalInfo.DestinationTo();

            Console.WriteLine();
            Console.WriteLine($"DO YOU WANT TO COLLECT ALL \"{inputSeason}\" THUMBNAILS? (ALL / N)");
            var isAllThumbs = getSeasonalInfo.isApproved();

            string frontSide = gc.frontSide;
            string backSide = gc.backSide;
            string pngExtension = gc.pngExtension;
            string[] allowedExtensions = gc.allowedExtensions;
            int thumbsCopied = 0;
            int thumbsNon = 0;

            ////Scan thumbs folder
            List<string> collectedPaths = getSeasonalInfo.ScanThumbsFolder(thumbnailsFolder, allowedExtensions);
            StringBuilder badGeometries = new StringBuilder();

            string inputGarment = "";

            if (isAllThumbs)
            {
                Console.WriteLine(TransferIfAll(inputGarment, frontSide, backSide, pngExtension, thumbnailsFolder,
                    destinationPath, thumbsCopied, thumbsNon, badGeometries, collectedPaths, geometryInUse,
                    inputSeason, gc, debug, printExport));
            }


            if (isAllThumbs == false)
            {
                Console.WriteLine(TransferIfAllFalse(inputGarment, frontSide, backSide, pngExtension, thumbnailsFolder,
                    destinationPath, thumbsCopied, thumbsNon, badGeometries, collectedPaths, geometryInUse,
                    inputSeason, gc, debug, printExport));
            }

            Console.WriteLine();
            Console.WriteLine($"DO YOU WANT TO GENERATE A LIST OF GEOEMETRIES/SKUS USED BY {inputSeason} ?: (Y / N)");
            var isGenerate = getSeasonalInfo.isApproved();

            if (isGenerate)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Console.WriteLine($"GENERATING LIST...");

                var allFilesJpg = getSeasonalInfo
                    .GetAllFilesAsync(gc.jpgExtension, debug.MenWomen, debug.YoungAthletes, debug.PlusSize);

                printExport.createOutputFileForEndOfSeason(validationPattern, allFilesJpg, inputSeason, gc.fileName, gc.xlsxExtension, debug.excelFilePath);
                Console.WriteLine("DONE!");
                sw.Stop();
                Console.WriteLine(sw.Elapsed);

            }
            else
            {
                Console.WriteLine("OK! Have a nice day!");
            }

            Console.ReadLine();
        }

        public string TransferIfAllFalse(string inputGarment, string frontSide, string backSide, string extension,
            string thumbnailsFolder, string destinationPath, int thumbsCopied, int thumbsNon, StringBuilder badGeometries,
            List<string> collectedPaths, HashSet<string> geometryInUse, string inputSeason, GlobalConstants gc, DDebugg debug, printAndExport printExport)
        {
            thumbsCopied = 0;
            thumbsNon = 0;
            bool isEnd;

            Console.WriteLine();
            Console.WriteLine("Insert garment code: ");

            while (true)
            {
                inputGarment = Console.ReadLine().ToUpper();

                //search for end variable
                isEnd = inputGarment == "END!" ? true : false;

                //if input is end! then break
                if (isEnd == true)
                {
                    break;
                }

                string splitThumbCounts = TransferCopies(inputGarment, frontSide, backSide, extension, thumbnailsFolder,
                    destinationPath, thumbsCopied, thumbsNon, badGeometries, collectedPaths);

                var split = splitThumbCounts
                    .Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                thumbsCopied += split[0];
                thumbsNon += split[1];
            }

            //print STATISTICS 
            printExport.resultsToFile(geometryInUse, badGeometries, inputSeason, gc, debug);
            string result = printExport.printResults(thumbsCopied, thumbsNon, badGeometries, geometryInUse);
            return result;
        }

        public string TransferIfAll(string inputGarment, string frontSide, string backSide, string extension, string thumbnailsFolder,
            string destinationPath, int thumbsCopied, int thumbsNon, StringBuilder badGeometries, List<string> collectedPaths,
            HashSet<string> geometryInUse, string inputSeason, GlobalConstants gc, DDebugg debug, printAndExport printExport)
        {
            thumbsCopied = 0;
            thumbsNon = 0;

            foreach (var geometry in geometryInUse)
            {
                //input garment code
                inputGarment = geometry;

                string splitThumbCounts = TransferCopies(inputGarment, frontSide, backSide, extension, thumbnailsFolder,
                    destinationPath, thumbsCopied, thumbsNon, badGeometries, collectedPaths);

                var split = splitThumbCounts
                    .Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                thumbsCopied += split[0];
                thumbsNon += split[1];
            }

            //print STATISTICS 
            printExport.resultsToFile(geometryInUse, badGeometries, inputSeason, gc, debug);
            string result = printExport.printResults(thumbsCopied, thumbsNon, badGeometries, geometryInUse);
            return result;
        }

        public static string TransferCopies(string inputGarment, string frontSide, string backSide, string extension, string thumbnailsFolder,
            string destinationPath, int thumbsCopied, int thumbsNon, StringBuilder badGeometries, List<string> collectedPaths
        )
        {
            thumbsCopied = 0;
            thumbsNon = 0;
            string garmentNameF = (inputGarment + frontSide + extension);
            string garmentNameB = (inputGarment + backSide + extension);

            //check/copy front
            if (File.Exists(thumbnailsFolder + garmentNameF)
                && collectedPaths.Contains(thumbnailsFolder + garmentNameF))
            {
                File.Copy(thumbnailsFolder + garmentNameF, destinationPath + garmentNameF, true);
                thumbsCopied += 1;
                Console.WriteLine("OK! - FRONT THUMB EXISTS");
            }
            else
            {
                thumbsNon += 1;
                badGeometries.AppendLine(" " + garmentNameF);
                Console.WriteLine("NO! - FRONT THUMB NOT EXISTS");

            }

            //check/copy back
            if (File.Exists(thumbnailsFolder + garmentNameB)
                && collectedPaths.Contains(thumbnailsFolder + garmentNameB))
            {
                File.Copy(thumbnailsFolder + garmentNameB, destinationPath + garmentNameB, true);
                thumbsCopied += 1;
                Console.WriteLine("OK! - BACK THUMB EXISTS");
            }
            else
            {
                thumbsNon += 1;
                badGeometries.AppendLine(" " + garmentNameB);
                Console.WriteLine("NO! - BACK THUMB NOT EXISTS");
            }

            string concatThumbsCounters = (thumbsCopied + "-" + thumbsNon).ToString();
            return concatThumbsCounters;
        }


    }
}
