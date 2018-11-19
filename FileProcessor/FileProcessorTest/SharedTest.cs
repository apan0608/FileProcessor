using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shared;

namespace FileProcessorTest
{
    [TestFixture]
    public class SharedTest
    {       
        [Test]
        public void FileContent_EmptyFileName_ThrowException()
        {
            Assert.That(() => new FileContent(null, null, null), Throws.Exception.TypeOf<ArgumentException>());
            Assert.That(() => new FileContent(string.Empty, null, null), Throws.Exception.TypeOf<ArgumentException>());
            Assert.That(() => new FileContent(" ", null, null), Throws.Exception.TypeOf<ArgumentException>());
        }
               
       [Test]
       public void FileContent_ContentNull_ReturnHasNoContent()
       {
           var fileContent = new FileContent("Test File", null, null);
           Assert.IsFalse(fileContent.HasContent);
           Assert.IsFalse(fileContent.HasHeader);
       }
        
        [Test]
        public void FileContent_ContentNotNull_ReturnHasContent()
        {
            var content = new List<string> {"1", "2", "3", "4"};
            var headerColumns = new List<string> {"1", "2", "3", "4"};
            var fileContent = new FileContent("Test File", new List<List<string>>{content}, headerColumns);
            Assert.IsTrue(fileContent.HasContent);
            Assert.IsTrue(fileContent.HasHeader);
        }
        
    }
}