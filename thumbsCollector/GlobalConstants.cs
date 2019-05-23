using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thumbsCollector
{
    public class GlobalConstants
    {
        public string MenWomen = @"M:\MD_N\Garments"; // the root folder for Men-Women-Equipment Garments
        public string YoungAthletes = @"M:\MD_N\Garments YA"; // the root folder for Young Athletes Garments
        public string PlusSize = @"M:\MD_N\Garments PS"; // the root folder for PlusSize Garments

        public string thumbnailsFolder = @"M:\MD_N\Thumbs\";

        public string frontSide = "-A"; //frontside suffix
        public string backSide = "-B"; //backside suffix
        public string extension = ".png";   //extension to search
        public string[] allowedExtensions = new[] { "-a.png", "-b.png" }; //allowed extensions
    }
}
