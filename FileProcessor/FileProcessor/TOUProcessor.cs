using Shared;

namespace FileProcessor
{
    public class TOUProcessor : FileProcessor
    {       
        public TOUProcessor()
        {
            _dataValueColumn = "Energy";
            _dateColumn = "Date/Time";
        }

    }
}