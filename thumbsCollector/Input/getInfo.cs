using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace thumbsCollector.Input
{
    public class getInfo
    {
        //flds
        private static GlobalConstants rootPath;

        //ctor
        public getInfo()
        {
            rootPath = new GlobalConstants();
        }

        //props
        private string PathMW => rootPath.MenWomen;
        private string PathYA => rootPath.YoungAthletes;
        private string PathPS => rootPath.PlusSize;

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

            //TODO MAKE NEXT ROWS MULTI-THREADING

            string[] filesPathsMWEQ = Directory.GetFiles(PathMW, "*.psd", SearchOption.AllDirectories); //getting the files
            string[] filesPathsYA = Directory.GetFiles(PathYA, "*.psd", SearchOption.AllDirectories); //getting the files
            string[] filesPathsPlusSize = Directory.GetFiles(PathPS, "*.psd", SearchOption.AllDirectories); //getting the files

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
