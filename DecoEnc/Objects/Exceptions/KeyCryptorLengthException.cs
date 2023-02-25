using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoEnc.Objects.Exceptions
{
    class KeyCryptorLengthException : Exception
    {
        public KeyCryptorLengthException(KeyCryptor key) : base($"Key length must be at least 4 digits")
        {

        }
    }
}
