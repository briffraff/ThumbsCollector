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
            Console.WriteLine($"GENERATE LIST OF GARMENT ID/SKU/STYLE USED BY {inputSeason}:");

            //TODO IMPLEMENTATION LOGIC FOR CREATING THIS LIST (Probably in excell sheet);

        }

        public static void printResults(int thumbsCopied, int thumbsNon, StringBuilder badGeometries)
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

        public static void resultsToFile(HashSet<string> geometryInUse, StringBuilder badGeometries, string inputSeason)
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
