using System;
using FileReader;

namespace DataExporter
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var directory = (args == null || args.Length == 0) ? string.Empty : args[0];

            if (string.IsNullOrEmpty(directory))
            {
                Console.WriteLine("Please specify the directory path.");
            }
            else
            {
                var reader = new CSVReader();
                var processor = new Processor(reader);
                processor.ReadFilesAndPrintResult(directory);
            }
        }
    }
}