using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoEnc.Objects
{
    class CryptFileResult
    {
       public struct CryptedFile
        {
            public CryptResult Result;
            public string FileName;
            public string Extension;
        }

        public IEnumerable<CryptedFile> Results = Enumerable.Empty<CryptedFile>();

        public CryptedFile OnlyFile { get => Results.First(); }

        public CryptFileResult(IEnumerable<CryptedFile> crypted)
        {
            Results = crypted;
        }
    }
}
