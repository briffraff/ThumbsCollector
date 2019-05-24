namespace thumbsCollector.Validations
{
    using System;

    public class Validator
    {
        private static string inputSeason;

        public Validator(string currentSeason)
        {
            inputSeason = currentSeason;
        }

        public string Pattern()
        {
            string validationPattern = @"\bT_(?<garment>[N|S]\d{2}[A-Z]\d{3})_(?<season>" + inputSeason + @")_(?<category>[A-Z][A-Z])_(?<sku>.{6})-.{3}_D\b";
            return validationPattern;
        }

        public void ValidateSeason()
        {
            string seasonSymbols = inputSeason.Remove(2);

            if (seasonSymbols != "SP" || seasonSymbols != "SU" || seasonSymbols != "HO" || seasonSymbols != "FA")
            {
                if (inputSeason.Length != 4)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Season \"{inputSeason}\" isn't correct!");

                    Environment.Exit(0);
                }

            }
        }

    }
}
