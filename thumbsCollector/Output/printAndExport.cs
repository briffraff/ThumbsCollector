using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace thumbsCollector.Output
{
    public class printAndExport
    {

        public static void createOutputFileForEndOfSeason(string inputSeason)
        {

            //TODO IMPLEMENTATION LOGIC FOR CREATING THIS LIST (Probably in excell sheet);

        }

        public static string printResults(int thumbsCopied, int thumbsNon, StringBuilder badGeometries,HashSet<string> geometryInUse)
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

        public static void resultsToFile(HashSet<string> geometryInUse, StringBuilder badGeometries, string inputSeason,GlobalConstants gc,DDebugg debug)
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
