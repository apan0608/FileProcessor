using System;
using System.IO;
using FileProcessor;
using FileReader;
using Shared;

namespace ConsoleApplication
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
                Processor.ProcessFiles(directory);
            }
        }

       
    }
}