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

        public string ValidateSeason()
        {


            while (inputSeason != null)
            {
                string seasonSymbols = string.Empty;

                if (inputSeason.Length >= 3)
                {
                    seasonSymbols = inputSeason.Remove(2);
                }


                if (inputSeason.Length == 4 && seasonSymbols == "SP" || seasonSymbols == "SU" || seasonSymbols == "HO" || seasonSymbols == "FA")
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Season \"{inputSeason}\" isn't correct! Try again!");
                    Console.WriteLine();
                    inputSeason = Console.ReadLine().ToUpper();
                }

            }

            Console.WriteLine("Correct!");
            return inputSeason;

        }

    }
}
