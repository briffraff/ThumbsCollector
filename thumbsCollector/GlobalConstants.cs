namespace thumbsCollector
{
    public class GlobalConstants
    {
        public string MenWomen = @"N:\Garments"; // the root folder for Men-Women-Equipment Garments
        public string YoungAthletes = @"N:\Garments YA"; // the root folder for Young Athletes Garments
        public string PlusSize = @"N:\Garments PS"; // the root folder for PlusSize Garments
        public string Matternity = @"N:\Garments MA";
        public string YAPS = @"N:\Garments YAPS";

        public string thumbnailsFolder = @"N:\MD_N\Thumbs\";
        public string pathToResults = @"M:\Z_Software Assets\3ds Max\BorakaScriptPack_vol1\assignmanager\ThumbsCollector\Results\";


        public string frontSide = "-A"; //frontside suffix
        public string backSide = "-B"; //backside suffix
        public string pngExtension = ".png"; //extension to search
        public string jpgExtension = ".jpg"; //extension to search
        public string psdExtension = ".psd"; //extension to search
        public string[] allowedExtensions = new[] { "-a.png", "-b.png" }; //allowed extensions

        public string excelFilePath = @"M:\Z_Software Assets\3ds Max\BorakaScriptPack_vol1\assignmanager\ThumbsCollector\Results\";
        public string fileName = "_OverallSeasonStatistic";
        public string xlsxExtension = ".xlsx";
    }

    public class DDebugg
    {
       public string MenWomen = @"G:\Projects\testFolder"; // debug
       public string YoungAthletes = @"G:\Projects\testFolder"; // debug
       public string PlusSize = @"G:\Projects\testFolder"; // debug
       public string thumbnailsFolder = @"G:\Projects\testFolder\thumbs\"; // debug
       public string pathToResults = @"G:\Projects\testFolder\results\";

       public string excelFilePath = @"G:\Projects\testFolder\results\";


    }
}

