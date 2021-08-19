using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using thumbsCollector.Core.Interfaces;
using thumbsCollector.Input;
using thumbsCollector.Output;
using thumbsCollector.Validations;
using System.Security.Principal;

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
                .GetAllFilesParralelAsync(gc.psdExtension, gc.MenWomen, gc.YoungAthletes, gc.PlusSize, gc.Matternity, gc.YAPS); //psd search

            var allFilesJpg = getSeasonalInfo
                .GetAllFilesParralelAsync(gc.jpgExtension, gc.MenWomen, gc.YoungAthletes, gc.PlusSize, gc.Matternity, gc.YAPS); //jpg search

            //main thumbs folder
            string thumbnailsFolder = gc.thumbnailsFolder;

            //enter directory where you want to save thumbs
            Console.WriteLine();
            Console.WriteLine("Please enter directory to save thumbs:".ToUpper());
            string destinationPath = getSeasonalInfo.DestinationTo();

            Console.WriteLine();
            Console.WriteLine($"DO YOU WANT TO COLLECT ALL \"{inputSeason}\" THUMBNAILS? (ALL / N)");
            var isAllThumbs = getSeasonalInfo.isApproved();

            var geometryInUse = getSeasonalInfo.GeometryInUse(allFilesPsd, validationPattern);

            string frontSide = gc.frontSide;
            string backSide = gc.backSide;
            string pngExtension = gc.pngExtension;
            string[] allowedExtensions = gc.allowedExtensions;
            int thumbsCopied = 0;
            int thumbsNon = 0;
            int thumbsAutoTransferA = 0;
            int thumbsAutoTransferB = 0;

            ////Scan thumbs folder
            List<string> collectedPaths = getSeasonalInfo.ScanThumbsFolder(thumbnailsFolder, allowedExtensions);


            // Normalize thumb's folder - transfer all not transfered thumbnails from maps folders of the garments
            var filepaths = allFilesPsd.Result
                .Select(x => Path.GetDirectoryName(x))
                .Select(x=>x.Replace("Maps","Renders"))
                .Distinct();

            Console.WriteLine();
            Console.WriteLine("AUTO RE-FILL:");
            Console.WriteLine();


            foreach (var filepath in filepaths)
            {
                var isFolderExists = Directory.Exists(filepath) ? true : false;
                var renderFolder = isFolderExists ? filepath : filepath.Replace("Renders", "Render");

                // Any other folder couldnt exists except Renders or Render
                if (Directory.Exists(renderFolder))
                {
                    var currentRenders = Directory.GetFiles(renderFolder, "*.png");
                    var pngfilesInFolder = currentRenders.Count();

                    if (pngfilesInFolder == 2)
                    {
                        //remove The last folder name and split
                        var garmentPath = (renderFolder.EndsWith("Renders")
                                ? renderFolder.Replace("Renders", "")
                                : renderFolder.Replace("Render", ""))
                            .Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();

                        //get garment folder name , split and get garment code
                        var garmentCode = garmentPath[garmentPath.Length - 1]
                            .Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];

                        var frontT = garmentCode + gc.frontSide + ".png";
                        var backT = garmentCode + gc.backSide + ".png";

                        //check if the same file exists in thumbs folder
                        var isFrontExists = File.Exists(thumbnailsFolder + frontT);
                        var isBackExists = File.Exists(thumbnailsFolder + backT);


                        if (!isFrontExists && currentRenders.Contains(Path.Combine(renderFolder, frontT)))
                        {
                            var srcFile = Path.Combine(renderFolder, frontT);
                            var destFile = Path.Combine(thumbnailsFolder, frontT);

                            var infoOwner = File.GetAccessControl(srcFile)
                                .GetOwner(typeof(NTAccount)).Value
                                .Replace("PIXELPOOL","");

                            File.Copy(srcFile, destFile);
                            Console.WriteLine("front : {0} - {1}",destFile,infoOwner);
                            thumbsAutoTransferA += 1;
                        }

                        if (!isBackExists && currentRenders.Contains(Path.Combine(renderFolder, backT)))
                        {
                            var srcFile = Path.Combine(renderFolder, backT);
                            var destFile = Path.Combine(thumbnailsFolder, backT);

                            var infoOwner = File.GetAccessControl(srcFile)
                                .GetOwner(typeof(NTAccount)).Value
                                .Replace("PIXELPOOL","");

                            File.Copy(srcFile, destFile);
                            Console.WriteLine("back : {0} - {1}",destFile,infoOwner);
                            thumbsAutoTransferB += 1;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
            Console.WriteLine();
            Console.WriteLine("transfered: ");
            Console.WriteLine(" => frontside : " + thumbsAutoTransferA);
            Console.WriteLine(" => backside : " + thumbsAutoTransferB);

            Console.ReadLine();

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

                while (true)
                {
                    if (allFilesJpg.IsCompleted == false)
                    {
                        continue;
                    }

                    break;
                }

                printExport.createOutputFileForEndOfSeasonAsync(validationPattern, allFilesJpg, inputSeason, gc.fileName, gc.xlsxExtension, gc.excelFilePath).Wait();

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
                //input garment path
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
