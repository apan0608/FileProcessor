using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace FileReader
{
    public class CSVReader: IFileReader
    {      
        private const char _separator = ',';
   
        public FileContent GetFileContent(string filePath, bool hasHeader)
        {          
            ValidateFile(filePath);
            return ReadFile(filePath, hasHeader);
        }

        private static void ValidateFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{filePath} cannot be found.");
            }   
        }

        private static FileContent ReadFile(string filePath, bool hasHeader)
        {
            var lines = new List<string[]>();
            var headers = new string[]{};

            using (var sr = new StreamReader(filePath))
            {
                string line;
               
                while ((line = sr.ReadLine()) != null)
                {                   
                    var elements = line.Split(_separator);

                    if (elements.Length <= 0) continue;
                    if (hasHeader && !headers.Any())
                    {
                        headers = elements;
                    }
                    else
                    {
                        lines.Add(elements);
                    }
                }
            }                          
            return new FileContent(filePath, lines, headers);
        }
    }
}