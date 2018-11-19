using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Shared;

namespace FileProcessor
{
    public abstract class FileProcessor
    {
        
        protected FileContent _Content;
        protected string _dataValueColumn;
        protected string _dateTimeStampColumn;
       
        private decimal _medianValue;
        private readonly int _PRECISION = 6;
        private int _dataValueColumnIndex;
        private int _dateTimeStampColumnIndex;
        
        protected IEnumerable<ProcessResult> _tempProcessSet;

        private FileProcessor()
        {
            throw new InvalidOperationException("Cannot initialize class without parameters.");        
        }
        
        public FileProcessor(string dataValueColumn, string dateTimeStampColumn)
        {
            _dataValueColumn = dataValueColumn;
            _dateTimeStampColumn = dateTimeStampColumn;
        }      

        public void SetDataAndPrepareProcessing(FileContent content)
        {
            ValidateFile(content);
            _Content = content;
            PrepareProcessing(); 
        }

        private void PrepareProcessing()
        {
            _dataValueColumnIndex =  GetColumnIndex(_dataValueColumn);
            _dateTimeStampColumnIndex = GetColumnIndex(_dateTimeStampColumn);
            if (_dataValueColumnIndex < 0 || _dateTimeStampColumnIndex < 0)
            {
                throw new InvalidDataException($"{_dataValueColumn} field or {_dateTimeStampColumn} field " +
                                               "does not exist in file.");
            }
            
            _tempProcessSet = PrepareResultSet();
            _medianValue = CalculateMedianValue();     
        }
        
        private void ValidateFile(FileContent content)
        {
            if (!content.HasHeader)
            {
                throw new InvalidDataException($"Header information is missing. Cannot process file {content.Name}.");
            }
        }
        
        private IEnumerable<ProcessResult> PrepareResultSet()
        {
            return _Content.Lines.Select(line => new ProcessResult(Convert.ToDecimal(line.ElementAt(_dataValueColumnIndex)),
                line.ElementAt(_dateTimeStampColumnIndex)));
        }
        
        protected decimal CalculateMedianValue()
        {
            if (_tempProcessSet == null)
            {
                throw new InvalidOperationException("Cannot get value at this stage.");
            }    
            return decimal.Round(_tempProcessSet.Average(line => line.Value), _PRECISION);           
        }

        protected int GetColumnIndex(string columnName)
        {
            return _Content.HeaderColumns.ToList().IndexOf(columnName);           
        }
        
        public ProcessResultSet GetBelowMedianByPercentage(decimal percentage)
        {
            ValidateProcess();
            var threshold = _medianValue * (1 - percentage);
            var processResults = _tempProcessSet.Where(line => line.Value < threshold);
            return new ProcessResultSet(_Content.Name, _medianValue, processResults);
        }
        
        public ProcessResultSet GetOverMedianByPercentage(decimal percentage)
        {
           ValidateProcess();
            var threshold = _medianValue * (1 + percentage);
            var processResults = _tempProcessSet.Where(line => line.Value > threshold);
            return new ProcessResultSet(_Content.Name, _medianValue, processResults);
        }

        private void ValidateProcess()
        {
            if (_Content == null)
            {
                throw new Exception("Cannot perform operation as file content has not been set yet.");
            }
        }
               
    }
}