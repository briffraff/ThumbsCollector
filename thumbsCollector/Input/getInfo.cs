using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace thumbsCollector.Input
{
    public class getInfo
    {

        public string currentSeason()
        {
            //validation in validator class
            string season = Console.ReadLine().ToUpper();
            return season;
        }

        public string DestinationTo()
        {
            string dest = Console.ReadLine();
            string destinationPath = !dest.EndsWith("\\") ? dest + "\\" : dest;

            while (dest != null)
            {
                bool firstChar = char.IsLetter(destinationPath[0]);
                bool secondChar = destinationPath[1] == ':';
                bool thirdChar = destinationPath[2] == '\\';

                if (dest.Length >= 3 && firstChar && secondChar && thirdChar)
                {
                    break;
                }

                Console.WriteLine("UNCORRECT INPUT ! Please review and try again!");
                Console.WriteLine();
                dest = Console.ReadLine();
                destinationPath = !dest.EndsWith("\\") ? dest + "\\" : dest;

            }

            return destinationPath;
        }

        public bool isApproved()
        {
            string yesOrNo = Console.ReadLine().ToUpper();
            var isApproved = yesOrNo == "ALL" || yesOrNo == "Y" ? true : false;

            while (yesOrNo != null)
            {
                if (isApproved)
                {
                    break;
                }
                else if (yesOrNo == "N")
                {
                    isApproved = false;
                    break;
                }
                else
                {
                    Console.WriteLine("*You must say 'Y' or 'N' !");

                    yesOrNo = Console.ReadLine().ToUpper();
                    isApproved = yesOrNo == "ALL" || yesOrNo == "Y" ? true : false;
                }
            }

            return isApproved;
        }

        public List<string> ScanThumbsFolder(string thumbnailsFolder, string[] allowedExtensions)
        {
            List<string> collectedPaths = new List<string>();

            collectedPaths = Directory.GetFiles(thumbnailsFolder, "*.*", SearchOption.AllDirectories)
                .Where(x => allowedExtensions.Any(x.ToLower().EndsWith)).ToList();

            return collectedPaths;
        }

        private void GetMenWomen(List<string> allFiles, string location, string extension)
        {
            var menWomenFiles = Directory.GetFiles(location, $"*{extension}", SearchOption.AllDirectories); //getting the files
            foreach (var path in menWomenFiles)
            {
                allFiles.Add(path);
            }

            Console.WriteLine($"MW - {menWomenFiles.Count()}");

        }

        private void GetYoungAthletes(List<string> allFiles, string location, string extension)
        {
            var youngAthletesFiles = Directory.GetFiles(location, $"*{extension}", SearchOption.AllDirectories); //getting the files
            foreach (var path in youngAthletesFiles)
            {
                allFiles.Add(path);
            }

            Console.WriteLine($"YA - {youngAthletesFiles.Count()}");
        }

        private void GetPlusSizes(List<string> allFiles, string location, string extension)
        {

            var plusSizesFiles = Directory.GetFiles(location, $"*{extension}", SearchOption.AllDirectories); //getting the files
            foreach (var path in plusSizesFiles)
            {
                allFiles.Add(path);
            }

            Console.WriteLine($"PS - {plusSizesFiles.Count()}");
        }

        public List<string> GetAllFiles(string extension, string PathMW, string PathYA, string PathPS)
        {
            List<string> allFiles = new List<string>();

            GetMenWomen(allFiles, PathMW, extension);
            GetYoungAthletes(allFiles, PathYA, extension);
            GetPlusSizes(allFiles, PathPS, extension);

            return allFiles;
        }

        public Task<List<string>> GetAllFilesAsync(string extension, string PathMW, string PathYA, string PathPS)
        {
            Task<List<string>> allFilesListTask = Task.Run(async () => GetAllFiles(extension, PathMW, PathYA, PathPS));

            return allFilesListTask;
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
