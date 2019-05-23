using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace thumbsCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("........................");
            Console.WriteLine("....THUMBS COLLECTOR....");
            Console.WriteLine("........................");
            Console.WriteLine();

            string pathRootMWEQ = @"M:\MD_N\Garments"; // the root folder for Men-Women-Equipment Garments
            string pathRootYA = @"M:\MD_N\Garments YA"; // the root folder for Young Athletes Garments
            string pathRootPlusSize = @"M:\MD_N\Garments PS"; // the root folder for PlusSize Garments

            Console.Write("SEASON :");
            Console.WriteLine();

            var inputSeason = Console.ReadLine().ToUpper();

            if (inputSeason.Length != 4)
            {
                Console.WriteLine();
                Console.WriteLine($"Season \"{inputSeason}\" isn't correct!");
            }

            string validationPattern = @"\bT_(?<garment>[N|S]\d{2}[A-Z]\d{3})_(?<season>" + inputSeason + @")_(?<category>[A-Z][A-Z])_(?<sku>.{6})-.{3}_D\b";

            List<string> allFiles = new List<string>();

            string[] filesPathsMWEQ = Directory.GetFiles(pathRootMWEQ, "*.psd", SearchOption.AllDirectories); //getting the files
            string[] filesPathsYA = Directory.GetFiles(pathRootYA, "*.psd", SearchOption.AllDirectories); //getting the files
            string[] filesPathsPlusSize = Directory.GetFiles(pathRootPlusSize, "*.psd", SearchOption.AllDirectories); //getting the files
            //string[] filesPathsFTW = Directory.GetFiles(pathRootFTW, "*.psd", SearchOption.AllDirectories); //getting the files

            //write the paths to the empty list
            foreach (var path in filesPathsMWEQ)
            {
                allFiles.Add(path);
            }

            foreach (var path in filesPathsYA)
            {
                allFiles.Add(path);
            }

            foreach (var path in filesPathsPlusSize)
            {
                allFiles.Add(path);
            }

            //foreach (var path in filesPathsFTW)
            //{
            //    allFiles.Add(path);
            //}

            HashSet<string> geometryInUse = new HashSet<string>();

            foreach (var file in allFiles)
            {
                var matches = Regex.Matches(file, validationPattern);

                foreach (Match match in matches)
                {
                    var currentGeometry = match.Groups["garment"].ToString();
                    geometryInUse.Add(currentGeometry);
                }
            }

            ;
            //main thumbs folder
            string thumbnailsFolder = @"M:\MD_N\Thumbs\";

            //enter directory where you want to save thumbs
            Console.WriteLine();
            Console.WriteLine("Please enter directory to save thumbs:".ToUpper());
            string dest = Console.ReadLine();
            string destinationPath = !dest.EndsWith("\\") ? dest + "\\" : dest;

            Console.WriteLine();
            Console.WriteLine($"DO YOU WANT TO COLLECT ALL \"{inputSeason}\" THUMBNAILS? (Y/N)");
            string yesOrNo = Console.ReadLine().ToUpper();
            var isAllThumbs = yesOrNo == "Y" ? true : false;

            string frontSide = "-A";
            string backSide = "-B";
            string extension = ".png";
            int thumbsCopied = 0;
            int thumbsNon = 0;
            bool isEnd;
            string[] allowedExtensions = new[] { "-a.png", "-b.png" };

            //Initialise the the lists
            List<string> collectedPaths = new List<string>();
            StringBuilder badGeometries = new StringBuilder();

            ////Scan the folders
            collectedPaths = Directory.GetFiles(thumbnailsFolder, "*.*", SearchOption.AllDirectories)
               .Where(x => allowedExtensions.Any(x.ToLower().EndsWith)).ToList();
            

            string inputGarment = "";

            if (isAllThumbs == true)
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

                //print STATISTICS 
                printResults(thumbsCopied, thumbsNon, badGeometries);
                resultsToFile(geometryInUse, badGeometries, inputSeason);
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

                //print STATISTICS 
                printResults(thumbsCopied, thumbsNon, badGeometries);
                resultsToFile(geometryInUse, badGeometries, inputSeason);
            }

            Console.WriteLine($"DO YOU WANT TO GENERATE A LIST OF GEOEMETRIES/SKUS USED BY {inputSeason} ?: (Y/N)");
            yesOrNo = Console.ReadLine().ToUpper();
            bool isGenerate = yesOrNo == "Y" ? true : false;

            if (isGenerate == true)
            {
                createOutputFileForEndOfSeason(inputSeason);
            }

            Console.ReadLine();
        }

       // private static List<string> 

        private static void createOutputFileForEndOfSeason(string inputSeason)
        {
            Console.WriteLine($"GENERATE LIST OF GEOEMETRIES/SKUS USED BY {inputSeason}:");

            //TODO IMPLEMENTATION LOGIC FOR CREATING THIS LIST (Probably in excell sheet);

        }

        private static void printResults(int thumbsCopied, int thumbsNon, StringBuilder badGeometries)
        {
            Console.WriteLine();
            Console.WriteLine($"Thumbs copied: {thumbsCopied} = {thumbsCopied / 2} garments");
            Console.WriteLine($"Thumbs non copied: {thumbsNon} = {thumbsNon / 2} garments");
            Console.WriteLine();

            if (thumbsNon != 0)
            {
                Console.WriteLine("[THUMBS MISSING:...");
                Console.WriteLine();
                Console.WriteLine(badGeometries);
            }
        }

        private static void resultsToFile(HashSet<string> geometryInUse, StringBuilder badGeometries, string inputSeason)
        {
            var pathToResults = @"M:\Z_Software Assets\3ds Max\BorakaScriptPack_vol1\assignmanager\ThumbsCollector\Results\";
            var resultFile = $"{inputSeason.ToUpper()} - geometries.txt";
            var badFileName = $"MissingThumbs - {inputSeason.ToUpper()}.txt";

            StringBuilder result = new StringBuilder();

            foreach (var geometry in geometryInUse)
            {
                result.AppendLine(geometry);
            }

            File.WriteAllText(pathToResults + resultFile, result.ToString());

            File.WriteAllText(pathToResults + badFileName, badGeometries.ToString());
        }

    }
}
