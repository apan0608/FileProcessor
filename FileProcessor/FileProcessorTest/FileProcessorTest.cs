using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shared;


namespace FileProcessorTest
{
    [TestFixture]
    public class FileProcessorTest
    {
        private FileContent GetTestingFileA()
        {
            var dataColumn = "Data Value";
            var dateTimeStampColumn = "Date/Time";

            var headerColumns = new List<string>
            {
                "MeterPoint Code",
                "Serial Number", "Plant Code", "Date/Time", "Data Type", "Data Value", "Units", "Status"
            };
          
            var lines = new List<List<string>>
            {
                new List<string>
                {
                    "210095893", "210095893", "ED031000001", "31/08/2015 00:45:00", "Import Wh Total", "26.590000",
                    "kwh", ""
                },
                new List<string>
                {
                    "210095893", "210095893", "ED031000001", "31/08/2015 01:00:00", "Import Wh Total", "36.420000",
                    "kwh", ""
                },
                new List<string>
                {
                    "210095893", "210095893", "ED031000001", "31/08/2015 01:15:00", "Import Wh Total", "32.900000",
                    "kwh", ""
                },
                new List<string>
                {
                    "210095893", "210095893", "ED031000001", "31/08/2015 01:30:00", "Import Wh Total", "22.940000",
                    "kwh", ""
                }
            };
            return new FileContent("file.csv", lines, headerColumns);
        }
            
        private FileContent GetTestingFileB()
        {
            var dataColumn = "Energy";
            var dateTimeStampColumn = "Date/Time";

            var headerColumns = new List<string>
            {
                "MeterPoint Code",
                "Serial Number", "Plant Code", "Date/Time", "Data Type", "Data Value", "Units", "Status"
            };
          
            var lines = new List<List<string>>
            {
                new List<string>
                {
                    "210095893", "210095893", "ED031000001", "31/08/2015 00:45:00", "Import Wh Total", "00.000000",
                    "kwh", ""
                },
                new List<string>
                {
                    "210095893", "210095893", "ED031000001", "31/08/2015 01:00:00", "Import Wh Total", "00.000000",
                    "kwh", ""
                },
                new List<string>
                {
                    "210095893", "210095893", "ED031000001", "31/08/2015 01:15:00", "Import Wh Total", "00.000000",
                    "kwh", ""
                }
            };       
              return new FileContent("fileB.csv", lines, headerColumns);
        }
        
        private FileContent GetTestingFileC()
        {
            var dataColumn = "Energy";
            var dateTimeStampColumn = "Date/Time";

            var headerColumns = new List<string>
            {
                "MeterPoint Code",
                "Serial Number", "Plant Code", "Date/Time", "Data Type", "Data Value", "Units", "Status"
            };
          
            var lines = new List<List<string>>
            {
                new List<string>
                {
                    "210095893", "210095893", "ED031000001", "31/08/2015 00:45:00", "Import Wh Total", "32.210000",
                    "kwh", ""
                },
                new List<string>
                {
                    "210095893", "210095893", "ED031000001", "31/09/2015 01:23:00", "Import Wh Total", "45.970000",
                    "kwh", ""
                },
                new List<string>
                {
                    "210095893", "210095893", "ED031000001", "31/08/2015 01:15:00", "Import Wh Total", "23.860000",
                    "kwh", ""
                }
            };       
            return new FileContent("fileC.csv", lines, headerColumns);
        }
        
        [Test]
        public void SetFileContent_HeaderMissing_ThrowException()
        {
            var file = new FileContent("file.csv", null, new List<string>());
            var processor = new MockingFileProcessor("data value column", "date time column");
            Assert.That(() => processor.SetDataAndPrepareProcessing(file),
                Throws.Exception.TypeOf<InvalidDataException>()
                    .With.Property("Message").Contains("Header information is missing."));           
        }
    
        [Test]
        public void SetFileContent_ColumnNotExist_ThrowException()
        {
            var columnExists = "Energy";
            var columnMissing = "Date/Time";
           
            var headers = new List<string>{"Column1", "Column2", columnExists, "Column4" };
            var file = new FileContent("file.csv", null, headers);
            var processor = new MockingFileProcessor(columnExists, columnMissing);
            Assert.That(() => processor.SetDataAndPrepareProcessing(file),
                Throws.Exception.TypeOf<InvalidDataException>()
                    .With.Property("Message").Contains("Date/Time field does not exist in file"));           
        }

        [Test]
        public void CalculateMedianValue_NotReady_TrowException()
        {
            var dataColumn = "Data Value";
            var dateTimeStampColumn = "Date/Time";
            
            var processor = new MockingFileProcessor(dataColumn, dateTimeStampColumn);
            
            Assert.That(() => processor.CalculateMedianValue(),
                Throws.Exception.TypeOf<InvalidOperationException>());   
        }
        
        [Test]
        public void CalculateMedianValue_Ready_MatchResult()
        {
            var dataColumn = "Data Value";
            var dateTimeStampColumn = "Date/Time";

            var file = GetTestingFileA();                           
            var processor = new MockingFileProcessor(dataColumn, dateTimeStampColumn);
            processor.SetDataAndPrepareProcessing(file);
            var medianValue = processor.CalculateMedianValue();

            decimal expected = (decimal)29.7125;
            Assert.AreEqual(medianValue, expected); 
        }

        
        [Test]
        public void GetColumnIndex_ColumnExist_MatchResult()
        {
            var dataColumn = "Data Value";
            var dateTimeStampColumn = "Date/Time";

            var file = GetTestingFileA();                           
            var processor = new MockingFileProcessor(dataColumn, dateTimeStampColumn);
            processor.SetDataAndPrepareProcessing(file);
            var index = processor.GetColumnIndex(dataColumn);

            var expected = 5;
            Assert.AreEqual(index, expected); 
        }

   
        [Test]
        public void GetColumnIndex_ColumnNotExist_NegativeIndex()
        {
            var dataColumn = "Data Value";
            var dateTimeStampColumn = "Date/Time";

            var file = GetTestingFileA();                           
            var processor = new MockingFileProcessor(dataColumn, dateTimeStampColumn);
            processor.SetDataAndPrepareProcessing(file);
            var index = processor.GetColumnIndex("Column not exists");

            var expected = -1;
            Assert.AreEqual(index, expected); 
        }

        [Test]
        public void GetBelowMedianByPercentage_20Percent_RecordFound()
        {
            var dataColumn = "Data Value";
            var dateTimeStampColumn = "Date/Time";

            var file = GetTestingFileA();                           
            var processor = new MockingFileProcessor(dataColumn, dateTimeStampColumn);
            processor.SetDataAndPrepareProcessing(file);

            var resultSet = processor.GetBelowMedianByPercentage((decimal)0.2);

            var median = (decimal)29.7125;
            Assert.AreEqual(resultSet.MedianValue, median);       
            Assert.AreEqual(resultSet.FileName, "file.csv");
            Assert.NotNull(resultSet.ProcessResults);
            Assert.AreEqual(resultSet.ProcessResults.Count(), 1);
            Assert.AreEqual(resultSet.ProcessResults.First().Value, (decimal)22.94);
            Assert.AreEqual(resultSet.ProcessResults.First().DateTimeStamp, "31/08/2015 01:30:00");
        }
        
        [Test]
        public void GetBelowMedianByPercentage_50Percent_RecordNotFound()
        {
            var dataColumn = "Data Value";
            var dateTimeStampColumn = "Date/Time";

            var file = GetTestingFileA();                           
            var processor = new MockingFileProcessor(dataColumn, dateTimeStampColumn);
            processor.SetDataAndPrepareProcessing(file);

            var resultSet = processor.GetBelowMedianByPercentage((decimal)0.5);

            var median = (decimal)29.7125;
            Assert.AreEqual(resultSet.MedianValue, median);       
            Assert.AreEqual(resultSet.FileName, "file.csv");
            Assert.NotNull(resultSet.ProcessResults);
            Assert.AreEqual(resultSet.ProcessResults.Count(), 0);
        }
        
        [Test]
        public void GetOverMedianByPercentage_10Percent_RecordFound()
        {
            var dataColumn = "Data Value";
            var dateTimeStampColumn = "Date/Time";

            var file = GetTestingFileA();                           
            var processor = new MockingFileProcessor(dataColumn, dateTimeStampColumn);
            processor.SetDataAndPrepareProcessing(file);

            var resultSet = processor.GetOverMedianByPercentage((decimal)0.1);

            var median = (decimal)29.7125;
            Assert.AreEqual(resultSet.MedianValue, median);       
            Assert.AreEqual(resultSet.FileName, "file.csv");
            Assert.NotNull(resultSet.ProcessResults);
            Assert.AreEqual(resultSet.ProcessResults.Count(), 2);
            Assert.IsTrue(resultSet.ProcessResults.Any(result => result.Value == (decimal)36.42 && result.DateTimeStamp == "31/08/2015 01:00:00"));
            Assert.IsTrue(resultSet.ProcessResults.Any(result => result.Value == (decimal)32.90 && result.DateTimeStamp == "31/08/2015 01:15:00"));            
        }
        
        [Test]
        public void GetOverMedianByPercentage_MultipleFiles_CorrectResults()
        {
            var dataColumn = "Data Value";
            var dateTimeStampColumn = "Date/Time";

            var fileA = GetTestingFileA();
            var fileB = GetTestingFileB();
            var fileC= GetTestingFileC();
            
            var processor = new MockingFileProcessor(dataColumn, dateTimeStampColumn);
            processor.SetDataAndPrepareProcessing(fileB);
            var resultSetB = processor.GetOverMedianByPercentage((decimal) 0.1);
            Assert.NotNull(resultSetB.ProcessResults);
            Assert.AreEqual(resultSetB.ProcessResults.Count(), 0);

            
            processor.SetDataAndPrepareProcessing(fileA);
            var resultSetA = processor.GetOverMedianByPercentage((decimal)0.1);

            var median = (decimal)29.7125;
            Assert.AreEqual(resultSetA.MedianValue, median);       
            Assert.AreEqual(resultSetA.FileName, "file.csv");
            Assert.NotNull(resultSetA.ProcessResults);
            Assert.AreEqual(resultSetA.ProcessResults.Count(), 2);
            Assert.IsTrue(resultSetA.ProcessResults.Any(result => result.Value == (decimal)36.42 && result.DateTimeStamp == "31/08/2015 01:00:00"));
            Assert.IsTrue(resultSetA.ProcessResults.Any(result => result.Value == (decimal)32.90 && result.DateTimeStamp == "31/08/2015 01:15:00"));   
            
            processor.SetDataAndPrepareProcessing(fileC);
            var resultSetC = processor.GetOverMedianByPercentage((decimal)0.2);

            median = (decimal)34.013333;
            Assert.AreEqual(resultSetC.MedianValue, median);       
            Assert.AreEqual(resultSetC.FileName, "fileC.csv");
            Assert.NotNull(resultSetC.ProcessResults);
            Assert.AreEqual(resultSetC.ProcessResults.Count(), 1);
            Assert.IsTrue(resultSetC.ProcessResults.Any(result => result.Value == (decimal)45.97 && result.DateTimeStamp == "31/09/2015 01:23:00"));

        }
    }
    
    
    
    
}