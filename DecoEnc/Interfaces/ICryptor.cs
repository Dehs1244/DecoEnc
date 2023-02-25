using DecoEnc.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DecoEnc.Interfaces
{
    interface ICryptor<T> : IDisposable where T : SymmetricAlgorithm, new()
    {
        T BaseCryptor { get; }
        KeyCryptor Key { get; }
        CryptResult Crypt(string input);

        static byte[] GetUnicodeBytes(string value) => Encoding.UTF8.GetBytes(value);

        void IDisposable.Dispose()
        {
            BaseCryptor.Dispose();
        }
    }
}
