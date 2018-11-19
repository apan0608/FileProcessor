using System.IO;
using System.Linq;
using System.Reflection;
using FileReader;
using NUnit.Framework;

namespace FileProcessorTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void CSVReader_FileNotExist_ThrowException()
        {
            var filePath = "/not exist";
            var reader = new CSVReader();

            Assert.That(() => reader.GetFileContent(filePath, true),
                Throws.Exception.TypeOf<FileNotFoundException>());
        }
        
        [Test]
        public void CSVReader_ReadFile_ContentWithHeader()
        {
            var localPath = "/TestFiles/LP.csv";         
            var filePath = GetFilePath(localPath);
            var content = (new CSVReader()).GetFileContent(filePath, true);

            Assert.NotNull(content.Lines);
            Assert.AreEqual(content.Lines.Count(), 4);
            Assert.AreEqual(content.HeaderColumns.Count(), 8);
        }
        
        [Test]
        public void CSVReader_ReadFile_ContentWithoutHeader()
        {
            var localPath = "/TestFiles/TOU.csv";         
            var filePath = GetFilePath(localPath);
            var content = (new CSVReader()).GetFileContent(filePath, false);

            Assert.NotNull(content.Lines);
            Assert.AreEqual(content.Lines.Count(), 3);
            var columns = content.Lines.First();
            Assert.AreEqual(columns.Count(), 15);
            Assert.AreEqual(content.HeaderColumns.Count(), 0);
        }
        
        [Test]
        public void CSVReader_ReadFile_Empty()
        {
            var localPath = "/TestFiles/EmptyFile.csv";
            var filePath = GetFilePath(localPath);
            var content = (new CSVReader()).GetFileContent(filePath, true);

            Assert.NotNull(content.Lines);
            Assert.AreEqual(content.Lines.Count(), 0);
            Assert.AreEqual(content.HeaderColumns.Count(), 0);
        }

        private string GetFilePath(string localFilePath)
        {
            return GetCurrentDirectory() + localFilePath;
        }

        private string GetCurrentDirectory()
        {          
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var testProjectRoot = Directory.GetParent(directory).Parent.FullName;
            return RemoveVirtualPath(testProjectRoot);       
        }

        private string RemoveVirtualPath(string path)
        {
            string virtualPathIndicator = "/file:";
            int indexOf = path.IndexOf(virtualPathIndicator);
            if (indexOf >= 0)
            {
                path = path.Substring(indexOf + virtualPathIndicator.Length);
            }
            return path;
        }
        
        
    }
}