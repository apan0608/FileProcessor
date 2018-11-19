using DataExporter;
using FileReader;
using NUnit.Framework;

namespace FileProcessorTest
{
    [TestFixture]
    public class ConsoleApplicationTest
    {
        [Test]
        public void ReadFilesAndPrintResult_FileNotExist_DoNothing()
        {
            var csvReader = new CSVReader();
            var processor = new Processor(csvReader);
            processor.ReadFilesAndPrintResult("not exists");           
        }       
    }
}