using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace FileProcessor
{
    public abstract class FileProcessor
    {
        protected FileContent _Content;
        protected string _dataValueColumn;
        protected string _dateColumn;
       
        private decimal? _medianValue;
        private int? _dataValueColumnIndex;
        private int? _dateColumnIndex;


        public FileContent Content
        {
            get => _Content;
            set
            {
                ValidateFile(value);
                _Content = value;
                PrepareResultSet();
            }
        }

        protected IEnumerable<ProcessResult> _InProcessSet;

        private int DataValueColumnIndex
        {
            get
            {
                if (!_dataValueColumnIndex.HasValue)
                {
                    _dataValueColumnIndex = GetColumnIndex(_dataValueColumn);              
                }
                return _dataValueColumnIndex.Value;
            }
        }
        
        private int DateColumnIndex
        {
            get
            {
                if (!_dateColumnIndex.HasValue)
                {
                    _dateColumnIndex = GetColumnIndex(_dateColumn);              
                }
                return _dateColumnIndex.Value;
            }
        }

        private decimal MedianValue
        {
            get
            {
                if (!_medianValue.HasValue)
                {
                    _medianValue = CalculateMedianValue();
                }

                return _medianValue.Value;
            }
        }

        protected void PrepareResultSet()
        {
            _InProcessSet = _Content.Lines.Select(line => new ProcessResult(Convert.ToDecimal(line.ElementAt(DataValueColumnIndex)),
                line.ElementAt(DateColumnIndex)));
        }

        protected void ValidateFile(FileContent content)
        {
            if (!content.HasHeader)
            {
                throw new InvalidDataException($"Header information is missing. Cannot process file {content.Name}.");
            }
        }

        protected decimal CalculateMedianValue()
        {
            return _InProcessSet.Average(line => line.Value);           
        }

        protected int GetColumnIndex(string columnName)
        {
            return _Content.Headers.ToList().IndexOf(columnName);           
        }
        
        public ProcessResultSet GetBelowMedianByPercentage(decimal percentage)
        {
            var threshold = MedianValue * (1 - percentage);
            var processResults = _InProcessSet.Where(line => line.Value < threshold);
            return new ProcessResultSet(_Content.Name, MedianValue, processResults);
        }
        
        public ProcessResultSet GetOverMedianByPercentage(decimal percentage)
        {
            var threshold = MedianValue * (1 + percentage);
            var processResults = _InProcessSet.Where(line => line.Value > threshold);
            return new ProcessResultSet(_Content.Name, MedianValue, processResults);
        }
               
    }
}