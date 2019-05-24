namespace thumbsCollector
{
    public class GlobalConstants
    {
        public string MenWomen = @"M:\MD_N\Garments"; // the root folder for Men-Women-Equipment Garments
        public string YoungAthletes = @"M:\MD_N\Garments YA"; // the root folder for Young Athletes Garments
        public string PlusSize = @"M:\MD_N\Garments PS"; // the root folder for PlusSize Garments

        public string thumbnailsFolder = @"M:\MD_N\Thumbs\";
        public string pathToResults = @"M:\Z_Software Assets\3ds Max\BorakaScriptPack_vol1\assignmanager\ThumbsCollector\Results\";


        public string frontSide = "-A"; //frontside suffix
        public string backSide = "-B"; //backside suffix
        public string pngExtension = ".png"; //extension to search
        public string jpgExtension = ".jpg"; //extension to search
        public string psdExtension = ".psd"; //extension to search
        public string[] allowedExtensions = new[] { "-a.png", "-b.png" }; //allowed extensions


    }

    public class DDebugg
    {
       public string MenWomen = @"G:\Projects\testFolder"; // debug
       public string YoungAthletes = @"G:\Projects\testFolder"; // debug
       public string PlusSize = @"G:\Projects\testFolder"; // debug
       public string thumbnailsFolder = @"G:\Projects\testFolder\thumbs\"; // debug
       public string pathToResults = @"G:\Projects\testFolder\results\";


    }
}

