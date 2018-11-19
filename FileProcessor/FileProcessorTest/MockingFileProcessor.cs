using Shared;

namespace FileProcessorTest
{
    public class MockingFileProcessor: FileProcessor.FileProcessor
    {
        
        public MockingFileProcessor(string dataValueColumn, string dateTimeStampColumn): 
            base(dataValueColumn, dateTimeStampColumn)
        {             
        }

        public void SetDataAndPrepareProcessing(FileContent content)
        {
            base.SetDataAndPrepareProcessing(content);
        }

        public int GetColumnIndex(string columnName)
        {
            return base.GetColumnIndex(columnName);
        }
            
        public decimal CalculateMedianValue()
        {
            return base.CalculateMedianValue();
        }
            
        public ProcessResultSet GetBelowMedianByPercentage(decimal percentage)
        {
            return base.GetBelowMedianByPercentage(percentage);
        }
   
        public ProcessResultSet GetOverMedianByPercentage(decimal percentage)
        {
            return base.GetOverMedianByPercentage(percentage);
        }
    }
}
