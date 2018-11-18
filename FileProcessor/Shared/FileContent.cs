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
        private readonly IEnumerable<string> _headers;

        public string Name => _name;
       
        public IEnumerable<IEnumerable<string>> Lines => _lines;
            
        // First line is header
        public bool HasContent => _lines.Any();
        
        public bool HasHeader => _headers.Any();

        public IEnumerable<string> Headers => _headers;

        private FileContent()
        {
            throw new InvalidOperationException("Cannot initialize this class without parameters");
        }

        public FileContent(string name, IEnumerable<IEnumerable<string>> lines, IEnumerable<string> header)
        {
            _name = name;
            _lines = lines;
            if (header.Any())
            {
                _headers = header;
            }
        }

    }
}