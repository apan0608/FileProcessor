using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class FileContent
    {
        private readonly string _name;
        private readonly IEnumerable<IEnumerable<string>> _lines;
        private readonly IEnumerable<string> _headerColumns;

        public string Name => _name;
       
        public IEnumerable<IEnumerable<string>> Lines => _lines;
            
        // First line is header
        public bool HasContent => _lines != null && _lines.Any();
        
        public bool HasHeader => _headerColumns != null &&_headerColumns.Any();

        public IEnumerable<string> HeaderColumns => _headerColumns;

        private FileContent()
        {
            throw new InvalidOperationException("Cannot initialize this class without parameters");
        }

        public FileContent(string name, IEnumerable<IEnumerable<string>> lines, IEnumerable<string> headerColumns)
        {
            if (name == null || string.IsNullOrEmpty(name.Trim()))
            {
                throw new ArgumentException("File name must be specified");
            }
            _name = name;
            _lines = lines;
            _headerColumns = headerColumns;
        }

    }
}