namespace Shared
{
    public class ProcessResult
    {        
        private readonly decimal _value;

        public decimal Value => _value;
       
        private readonly string _dateTimeStamp;

        public string DateTimeStamp => _dateTimeStamp;


        public ProcessResult(decimal value, string dateTimeStamp)
        {
            _value = value;
            _dateTimeStamp = dateTimeStamp;
        }
    }
}