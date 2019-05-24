using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

            Console.WriteLine("........................");
            Console.WriteLine("....THUMBS COLLECTOR....");
            Console.WriteLine("........................");
            Console.WriteLine();

            Console.Write("SEASON :");
            Console.WriteLine();

            getInfo getSeasonalInfo = new getInfo();
            string inputSeason = getSeasonalInfo.currentSeason();

            Validator validator = new Validator(inputSeason);
            validator.ValidateSeason();
            string validationPattern = validator.Pattern();

            var allFiles = getSeasonalInfo.AllFiles();
            var geometryInUse = getSeasonalInfo.GeometryInUse(allFiles, validationPattern);

            //main thumbs folder
            string thumbnailsFolder = gc.thumbnailsFolder;

            //enter directory where you want to save thumbs
            Console.WriteLine();
            Console.WriteLine("Please enter directory to save thumbs:".ToUpper());
            string destinationPath = getSeasonalInfo.DestinationTo();

            Console.WriteLine();
            Console.WriteLine($"DO YOU WANT TO COLLECT ALL \"{inputSeason}\" THUMBNAILS? (Y/N)");
            var isAllThumbs = getSeasonalInfo.isApproved();

            string frontSide = gc.frontSide;
            string backSide = gc.backSide;
            string extension = gc.extension;
            string[] allowedExtensions = gc.allowedExtensions;
            int thumbsCopied = 0;
            int thumbsNon = 0;
            bool isEnd;

            ////Scan thumbs folder
            List<string> collectedPaths = getSeasonalInfo.ScanThumbsFolder(thumbnailsFolder, allowedExtensions);
            StringBuilder badGeometries = new StringBuilder();

            string inputGarment = "";

            if (isAllThumbs)
            {
                foreach (var geometry in geometryInUse)
                {

                    //input garment code
                    inputGarment = geometry;

                    //search for end variable
                    isEnd = inputGarment == "END!" ? true : false;

                    //if input is end! then break
                    if (isEnd == true)
                    {
                        break;
                    }

                    TraceCopying(inputGarment, frontSide, backSide, extension, thumbnailsFolder,
                        destinationPath, thumbsCopied, thumbsNon, badGeometries, collectedPaths);
                }

                //print STATISTICS 
                printAndExport.printResults(thumbsCopied, thumbsNon, badGeometries);
                printAndExport.resultsToFile(geometryInUse, badGeometries, inputSeason);
            }

            if (isAllThumbs == false)
            {
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

                    TraceCopying(inputGarment, frontSide, backSide, extension, thumbnailsFolder,
                        destinationPath, thumbsCopied, thumbsNon, badGeometries, collectedPaths);

                }

                //print STATISTICS 
                printAndExport.printResults(thumbsCopied, thumbsNon, badGeometries);
                printAndExport.resultsToFile(geometryInUse, badGeometries, inputSeason);
            }

            Console.WriteLine($"DO YOU WANT TO GENERATE A LIST OF GEOEMETRIES/SKUS USED BY {inputSeason} ?: (Y/N)");
            var isGenerate = getSeasonalInfo.isApproved();

            if (isGenerate)
            {
                printAndExport.createOutputFileForEndOfSeason(inputSeason);
            }

            Console.ReadLine();
        }

        public static void TraceCopying(string inputGarment, string frontSide, string backSide, string extension, string thumbnailsFolder,
            string destinationPath, int thumbsCopied, int thumbsNon, StringBuilder badGeometries, List<string> collectedPaths
        )
        {
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
        }
    }
}
