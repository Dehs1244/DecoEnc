using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecoEnc.Interfaces;
using System.Security.Cryptography;
using System.IO;

namespace DecoEnc.Objects
{
    class Decryptor : IDecryptor<AesManaged>
    {
        public AesManaged BaseDecryptor { get; }

        public KeyCryptor Key { get; }
        public CryptSettings CryptSettings;

        public Decryptor(KeyCryptor key)
        {
            BaseDecryptor = new AesManaged();
            Key = key;
            BaseDecryptor.Mode = CipherMode.CBC;
        }

        public DecryptResult Decrypt(string input)
        {
            CryptSettings ??= CryptSettings.Instance.Value;
            byte[] vectorBytes = IDecryptor<AesManaged>.GetUnicodeBytes(CryptSettings.Vector);
            byte[] saltBytes = IDecryptor<AesManaged>.GetUnicodeBytes(CryptSettings.Salt);
            byte[] valueBytes = Convert.FromBase64String(input);

            byte[] decrypted;
            int decryptedByteCount = 0;

            PasswordDeriveBytes _passwordBytes =
                new PasswordDeriveBytes(Key.Password, saltBytes, CryptSettings.Hash, CryptSettings.Iterations);
            byte[] keyBytes = _passwordBytes.GetBytes(CryptSettings.KeySize / 8);

            try
            {
                using (ICryptoTransform transformEncryptor = BaseDecryptor.CreateDecryptor(keyBytes, vectorBytes))
                {
                    using (MemoryStream baseStream = new MemoryStream(valueBytes))
                    {
                        using (CryptoStream reader = new CryptoStream(baseStream, transformEncryptor, CryptoStreamMode.Read))
                        {
                            decrypted = new byte[valueBytes.Length];
                            decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                        }
                    }
                }
            }
            catch
            {
                return new DecryptResult() { DecryptedValue = string.Empty };
            }
            finally
            {
                BaseDecryptor.Clear();
            }

            return new DecryptResult() { Bytes = decrypted, DecryptedValue = Encoding.UTF8.GetString(decrypted) };
        }
    }
}
