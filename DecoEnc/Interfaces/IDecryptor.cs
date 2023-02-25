using DecoEnc.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DecoEnc.Interfaces
{
    interface IDecryptor<T> : IDisposable where T : SymmetricAlgorithm, new()
    {
        T BaseDecryptor { get; }
        KeyCryptor Key { get; }

        static byte[] GetUnicodeBytes(string value) => ICryptor<T>.GetUnicodeBytes(value);

        DecryptResult Decrypt(string input);

        void IDisposable.Dispose()
        {
            BaseDecryptor.Dispose();
        }
    }
}
