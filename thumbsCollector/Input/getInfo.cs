using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace thumbsCollector.Input
{
    public class getInfo
    {
        //flds
        private static GlobalConstants rootPath;
        private static DDebugg debug; 

        //ctor
        public getInfo()
        {
            rootPath = new GlobalConstants();
            debug = new DDebugg();
        }

        //props
        private string PathMW => debug.MenWomen;
        private string PathYA => debug.YoungAthletes;
        private string PathPS => debug.PlusSize;

        public string currentSeason()
        {
            string season = Console.ReadLine().ToUpper();
            return season;
        }


        public string DestinationTo()
        {
            string dest = Console.ReadLine();
            string destinationPath = !dest.EndsWith("\\") ? dest + "\\" : dest;

            return destinationPath;
        }

        public bool isApproved()
        {
            string yesOrNo = Console.ReadLine().ToUpper();
            var isApproved = yesOrNo == "Y" ? true : false;

            return isApproved;
        }


        public List<string> ScanThumbsFolder(string thumbnailsFolder,string[] allowedExtensions)
        {
            List<string> collectedPaths = new List<string>();

            collectedPaths = Directory.GetFiles(thumbnailsFolder, "*.*", SearchOption.AllDirectories)
                .Where(x => allowedExtensions.Any(x.ToLower().EndsWith)).ToList();

            return collectedPaths;
        }

        public List<string> AllFiles()
        {
            List<string> allFiles = new List<string>();

            //TODO --MAKE NEXT ROWS MULTI-THREADING

            string[] filesPathsMWEQ = Directory.GetFiles(PathMW, $"*{rootPath.psdExtension}", SearchOption.AllDirectories); //getting the files
            //string[] filesPathsYA = Directory.GetFiles(PathYA, $"*{rootPath.psdExtension}", SearchOption.AllDirectories); //getting the files
            //string[] filesPathsPlusSize = Directory.GetFiles(PathPS, $"*{rootPath.psdExtension}", SearchOption.AllDirectories); //getting the files

            //write the paths to the empty list
            foreach (var path in filesPathsMWEQ)
            {
                allFiles.Add(path);
            }

            //foreach (var path in filesPathsYA)
            //{
            //    allFiles.Add(path);
            //}

            //foreach (var path in filesPathsPlusSize)
            //{
            //    allFiles.Add(path);
            //}

            return allFiles;
        }

        public HashSet<string> GeometryInUse(List<string> allFiles, string validationPattern)
        {

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

            return geometryInUse;
        }
    }
}
