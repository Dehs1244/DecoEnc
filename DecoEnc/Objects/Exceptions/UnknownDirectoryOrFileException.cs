using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoEnc.Objects.Exceptions
{
    class UnknownDirectoryOrFileException : Exception
    {
        public UnknownDirectoryOrFileException(string path, bool isDirectory) : base(isDirectory ? $"Unknown directory \"{path}\"" : $"Unknown file {path}")
        {

        }
    }
}
