using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
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
                if (dest.Length >= 3)
                {
                    bool firstChar = char.IsLetter(destinationPath[0]);
                    bool secondChar = destinationPath[1] == ':';
                    bool thirdChar = destinationPath[2] == '\\';

                    if (firstChar && secondChar && thirdChar)
                    {
                        break;
                    }
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

        private async Task GetMenWomenAsync(List<string> allFiles, string location, string extension)
        {
            var menWomenFiles = await Task.Run(() => Directory.GetFiles(location, $"*{extension}", SearchOption.AllDirectories)); //getting the files
            foreach (var path in menWomenFiles)
            {
                allFiles.Add(path);
            }

            Console.WriteLine($"MW - {menWomenFiles.Count()}");

        }

        private async Task GetYoungAthletesAsync(List<string> allFiles, string location, string extension)
        {
            var youngAthletesFiles = await Task.Run(() => Directory.GetFiles(location, $"*{extension}", SearchOption.AllDirectories)); //getting the files
            foreach (var path in youngAthletesFiles)
            {
                allFiles.Add(path);
            }

            Console.WriteLine($"YA - {youngAthletesFiles.Count()}");
        }

        private async Task GetPlusSizesAsync(List<string> allFiles, string location, string extension)
        {

            var plusSizesFiles = await Task.Run(() => Directory.GetFiles(location, $"*{extension}", SearchOption.AllDirectories)); //getting the files
            foreach (var path in plusSizesFiles)
            {
                allFiles.Add(path);
            }

            Console.WriteLine($"PS - {plusSizesFiles.Count()}");
        }

        private async Task GetMAternityAsync(List<string> allFiles, string location, string extension)
        {

            var maternityFiles = await Task.Run(() => Directory.GetFiles(location, $"*{extension}", SearchOption.AllDirectories)); //getting the files
            foreach (var path in maternityFiles)
            {
                allFiles.Add(path);
            }

            Console.WriteLine($"MA - {maternityFiles.Count()}");
        }

        private async Task GetYAPSAsync(List<string> allFiles, string location, string extension)
        {

            var yapsFiles = await Task.Run(() => Directory.GetFiles(location, $"*{extension}", SearchOption.AllDirectories)); //getting the files
            foreach (var path in yapsFiles)
            {
                allFiles.Add(path);
            }

            Console.WriteLine($"YAPS - {yapsFiles.Count()}");
        }

        public async Task<List<string>> GetAllFilesAsync(string extension, string PathMW, string PathYA, string PathPS, string PathMA, string PathYAPS)
        {
            List<string> allFilesAsync = new List<string>();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            await GetMenWomenAsync(allFilesAsync, PathMW, extension);
            await GetYoungAthletesAsync(allFilesAsync, PathYA, extension);
            await GetPlusSizesAsync(allFilesAsync, PathPS, extension);
            await GetMAternityAsync(allFilesAsync, PathMA, extension);
            await GetYAPSAsync(allFilesAsync, PathYAPS, extension);


            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            return allFilesAsync;

        }

        public async Task<List<string>> GetAllFilesParralelAsync(string extension, string PathMW, string PathYA, string PathPS, string PathMA, string PathYAPS)
        {
            List<string> allFilesAsync = new List<string>();

            List<Task> tasks = new List<Task>();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            tasks.Add((Task.Run(() => GetMenWomenAsync(allFilesAsync, PathMW, extension))));
            tasks.Add((Task.Run(() => GetYoungAthletesAsync(allFilesAsync, PathYA, extension))));
            tasks.Add((Task.Run(() => GetPlusSizesAsync(allFilesAsync, PathPS, extension))));
            tasks.Add((Task.Run(() => GetMAternityAsync(allFilesAsync, PathMA, extension))));
            tasks.Add((Task.Run(() => GetYAPSAsync(allFilesAsync, PathYAPS, extension))));


            await Task.WhenAll(tasks);

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            return allFilesAsync;
        }

        public HashSet<string> GeometryInUse(Task<List<string>> allFiles, string validationPattern)
        {
            HashSet<string> geometryInUse = new HashSet<string>();

            foreach (var file in allFiles.Result)
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
