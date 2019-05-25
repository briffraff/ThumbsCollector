using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using thumbsCollector.Excel;

namespace thumbsCollector.Output
{
    public class printAndExport
    {

        public void createOutputFileForEndOfSeason(string validationPattern, List<string> allFilesXlsx, string inputSeason, string fileName, string extension, string excelFilePath)
        {

            //start init the xlsx file
            ExcelApp seasonStatisticApp;

            //GetFiles
            var allFiles = allFilesXlsx;

            if (File.Exists(excelFilePath + fileName + extension) == false)
            {
                //create file
                ExcelApp createNewApp = new ExcelApp();
                createNewApp.CreateNewFile();
                createNewApp.SaveAs($@"{excelFilePath}{fileName}{extension}");
                createNewApp.Close();

                //init and rename
                seasonStatisticApp = new ExcelApp(excelFilePath + fileName + extension, 1);
                seasonStatisticApp.RenameSheet(inputSeason);
            }
            else
            {
                //init
                seasonStatisticApp = new ExcelApp(excelFilePath + fileName + extension, 1);
                //check if sheet exists
                bool isExist = seasonStatisticApp.ContainsSheet(inputSeason);

                if (isExist == false)
                {
                    //if not add
                    seasonStatisticApp.AddNewWorksheet(inputSeason);
                }
                else
                {
                    //if yes select it ,clear and overwrite 
                    seasonStatisticApp.SelectSheet(inputSeason);
                    seasonStatisticApp.ClearSheet();
                    Console.WriteLine($"Overwriten tab: [{seasonStatisticApp.GetSheetId()}]");

                }

            }

            //star writing in the xlsx file

            seasonStatisticApp.Write(0, 0, "[ " + inputSeason + " ]");
            seasonStatisticApp.Write(1, 0, "GARMENT ID");
            seasonStatisticApp.Write(1, 1, "SKU");
            seasonStatisticApp.Write(1, 2, "STYLE");


            int row = 2;
            int col = 0;
            //int filesMax = allFiles.Count;
            //int onePercent = 100 / filesMax;
            //int percentsDone = 0;

            foreach (var file in allFiles)
            {
                var matches = Regex.Matches(file, validationPattern);

                foreach (Match match in matches)
                {
                    var currentGarmentId = match.Groups["garment"].ToString();
                    var currentStyle = match.Groups["skuStyle"].ToString();
                    var currentSku = currentStyle + "-" + match.Groups["colorCode"];

                    seasonStatisticApp.Write(row, col, currentGarmentId);
                    col++;
                    seasonStatisticApp.Write(row, col, currentSku);
                    col++;
                    seasonStatisticApp.Write(row, col, currentStyle);
                    col++;

                    //reset col
                    col = 0;

                    //increase row
                    row++;
                }
            }

            //counter of skus
            seasonStatisticApp.Write(0, 3, $"COUNTER[ {seasonStatisticApp.Counter()} ]");

            //format cells and columns
            seasonStatisticApp.Formatting();
            seasonStatisticApp.Save();
            seasonStatisticApp.Close();
        }

        public string printResults(int thumbsCopied, int thumbsNon, StringBuilder badGeometries, HashSet<string> geometryInUse)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(Environment.NewLine);
            sb.AppendLine($"Thumbs copied: {thumbsCopied} = {thumbsCopied / 2} garments");
            sb.AppendLine($"Thumbs non copied: {thumbsNon} = {thumbsNon / 2} garments");
            sb.AppendLine($"Unique geometries : {geometryInUse.Count}");
            sb.AppendLine(Environment.NewLine);

            if (thumbsNon != 0)
            {
                sb.AppendLine("[THUMBS MISSING:...");
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine(badGeometries.ToString());
            }

            string result = sb.ToString().TrimEnd();
            return result;
        }

        public void resultsToFile(HashSet<string> geometryInUse, StringBuilder badGeometries, string inputSeason, GlobalConstants gc, DDebugg debug)
        {
            var pathToResults = debug.pathToResults;
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
