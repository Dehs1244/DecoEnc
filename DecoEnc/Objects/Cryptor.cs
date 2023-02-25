using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DecoEnc.Interfaces;

namespace DecoEnc.Objects
{
    class Cryptor : ICryptor<AesManaged>
    {
        public KeyCryptor Key { get; }

        public AesManaged BaseCryptor { get; }

        public Cryptor(string key)
        {
            BaseCryptor = new AesManaged();
            Key = new KeyCryptor(key);
            BaseCryptor.Mode = CipherMode.CBC;
        }

        public CryptResult Crypt(string input)
        {
            CryptSettings cryptSettings = CryptSettings.Instance.Value;
            byte[] vectorBytes = ICryptor<AesManaged>.GetUnicodeBytes(cryptSettings.Vector);
            byte[] saltBytes = ICryptor<AesManaged>.GetUnicodeBytes(cryptSettings.Salt);
            byte[] valueBytes = ICryptor<AesManaged>.GetUnicodeBytes(input);

            byte[] crypted;

            PasswordDeriveBytes _passwordBytes = 
                new PasswordDeriveBytes(Key.Password, saltBytes, cryptSettings.Hash, cryptSettings.Iterations);
            byte[] keyBytes = _passwordBytes.GetBytes(cryptSettings.KeySize / 8);

            using (ICryptoTransform transformEncryptor = BaseCryptor.CreateEncryptor(keyBytes, vectorBytes))
            {
                using (MemoryStream baseStream = new MemoryStream())
                {
                    using (CryptoStream writer = new CryptoStream(baseStream, transformEncryptor, CryptoStreamMode.Write))
                    {
                        writer.Write(valueBytes, 0, valueBytes.Length);
                        writer.FlushFinalBlock();
                        crypted = baseStream.ToArray();
                    }
                }
            }
            BaseCryptor.Clear();

            return new CryptResult() { Bytes = crypted, CryptedValue = Convert.ToBase64String(crypted) };
        }
    }
}
