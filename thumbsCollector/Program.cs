using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using thumbsCollector.Core;
using thumbsCollector.Input;
using thumbsCollector.Output;
using thumbsCollector.Validations;

namespace thumbsCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine engine = new Engine();
            engine.Run();
        }
    }
}
