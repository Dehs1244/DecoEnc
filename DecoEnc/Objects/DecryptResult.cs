using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoEnc.Objects
{
    class DecryptResult
    {
        public byte[] Bytes;
        public string DecryptedValue;
        public bool Succesfull { get => !string.IsNullOrEmpty(DecryptedValue); }
    }
}
