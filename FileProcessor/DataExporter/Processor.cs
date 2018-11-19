using System;
using System.Collections.Generic;
using System.IO;
using FileProcessor;
using FileReader;
using Shared;

namespace DataExporter
{
    public class Processor
    {
        private readonly IFileReader _reader;
        private readonly Dictionary<FileType, FileProcessor.FileProcessor> _fileProcessors;

        public Processor(IFileReader reader)
        {
            _reader = reader;
            _fileProcessors = new Dictionary<FileType, FileProcessor.FileProcessor>();
        }
        
        public void ReadFilesAndPrintResult(string directory)
        {
            try
            {
                var files = Directory.GetFiles(directory);                           
                Console.WriteLine($"Start processing files in directory {directory}.");
                foreach (var filePath in files)
                {                    
                    var processor = GetFileProcessor(filePath);
                    if (processor == null)
                        continue;
                    var resultSets = ProcessFile(_reader, processor, filePath);
                    foreach (var resultSet in resultSets)
                    {
                        PrintResults(resultSet);                        
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Failed to process files: {exc}"); 
            }
        }
        
        public IEnumerable<ProcessResultSet> ProcessFile(IFileReader reader, FileProcessor.FileProcessor processor, string filePath)
        {
            var resultSets = new List<ProcessResultSet>();
            var hasHeader = true;
            var content = reader.GetFileContent(filePath, hasHeader);
            if (content.HasContent)
            {
                decimal percentage = (decimal)0.2;
                processor.SetDataAndPrepareProcessing(content);                
                resultSets.Add(processor.GetBelowMedianByPercentage(percentage));
                resultSets.Add(processor.GetOverMedianByPercentage(percentage));
            }

            return resultSets;
        }

        private FileProcessor.FileProcessor GetFileProcessor(string filePath)
        {
            FileProcessor.FileProcessor fileProcessor = null;

            string fileName = Path.GetFileName(filePath);
            var fileType = GetFileType(fileName);
            if (fileType != FileType.UNKNOWN)
            {
                fileProcessor = GetFileProcessorFromCache(fileType);
            }

            return fileProcessor;
        }

        private FileProcessor.FileProcessor GetFileProcessorFromCache(FileType fileType)
        {
            if (!_fileProcessors.ContainsKey(fileType))
            {                
                var fileProcessor = CreateFileProcessor(fileType);
                _fileProcessors.Add(fileType, fileProcessor);
            }
            return _fileProcessors[fileType];
        }
        
        private FileProcessor.FileProcessor CreateFileProcessor(FileType fileType)
        {
            if (fileType == FileType.LP)
            {
                return new LPProcessor();
            }
            else if (fileType == FileType.TOU)
            {
                return new TOUProcessor();
            }
            else
            {
                return null;
            }
        }

        private FileType GetFileType(string fileName)
        {
            var fileType = FileType.UNKNOWN;

            if (!string.IsNullOrEmpty(fileName))
            {
                var fileTypes = Enum.GetValues(typeof(FileType));

                foreach (FileType type in fileTypes)
                {
                    var identifier = type.ToString();
                    if (fileName.Length > identifier.Length &&
                        fileName.Substring(0, identifier.Length) == identifier)
                    {
                        fileType = type;
                        break;
                    }
                }
            }
            return fileType;
        }
        
        private void PrintResults(ProcessResultSet resultSet)
        {
            foreach (var result in resultSet.ProcessResults)
            {
                Console.WriteLine($"{resultSet.FileName} {result.DateTimeStamp} {result.Value} {resultSet.MedianValue}");
            }
        }

    }
}