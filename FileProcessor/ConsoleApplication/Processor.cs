using System;
using System.IO;
using FileProcessor;
using FileReader;
using Shared;

namespace ConsoleApplication
{
    public static class Processor
    {
        public static void ProcessFiles(string directory)
        {
            try
            {
                var files = Directory.GetFiles(directory);
                foreach (var filePath in files)
                {
                    IFileReader reader = new CSVReader();
                    var processor = GetFileProcessor(filePath);
                    var hasHeader = true;
                    var content = reader.GetFileContent(filePath, hasHeader);
                  
                    if (processor != null && content.HasContent)
                    {
                        ProcessFileAndPrint(processor, content);
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Failed to process files: {exc}"); 
            }
        }

        private static FileProcessor.FileProcessor GetFileProcessor(string filePath)
        {
            FileProcessor.FileProcessor fileProcessor = null;
                        
            if (IsGivenFileType(filePath, FileType.LP))
            {                                                   
                fileProcessor = new LPProcessor();                                              
            } else if (IsGivenFileType(filePath, FileType.TOU))
            {               
                fileProcessor = new TOUProcessor();
            }
            return fileProcessor;
        }       
        
        private static bool IsGivenFileType(string fileName, FileType fileType)
        {
            var identifier = fileType.ToString();
            return fileName != null && fileName.Length > identifier.Length &&
                   fileName.Substring(0, identifier.Length) == identifier;
        }

        private static void ProcessFileAndPrint(FileProcessor.FileProcessor processor, FileContent content)
        {
            decimal percentage = (decimal)0.2;
            processor.Content = content;
            var resultSet = processor.GetBelowMedianByPercentage(percentage);
            string message = $"Print data below median value by {percentage * 100}%";
            PrintResults(message, resultSet);
            
            resultSet = processor.GetOverMedianByPercentage(percentage);
            message = $"Print data over median value by {percentage * 100}%";
            PrintResults(message, resultSet);
        }
        

        private static void PrintResults(string message, ProcessResultSet resultSet)
        {
            Console.WriteLine(message);
            foreach (var result in resultSet.ProcessResults)
            {
                Console.WriteLine($"{resultSet.FileName} {result.DateTimeStamp} {result.Value} {resultSet.MedianValue}");
            }
        }

    }
}