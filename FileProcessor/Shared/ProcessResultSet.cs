using System;
using System.Collections;
using System.Collections.Generic;

namespace Shared
{
    public class ProcessResultSet
    {
        private readonly string _fileName;    
        public string FileName => _fileName;
                
        private readonly decimal _medianValue;
        public decimal MedianValue => _medianValue;

        private readonly IEnumerable<ProcessResult> _processResults;

        public IEnumerable<ProcessResult> ProcessResults => _processResults;

        public ProcessResultSet(string fileName, decimal medianValue, IEnumerable<ProcessResult> processResults)
        {
            _fileName = fileName;
            _medianValue = medianValue;
            _processResults = processResults;
        }
    }
}