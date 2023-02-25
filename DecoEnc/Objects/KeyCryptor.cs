using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using DecoEnc.Objects.Exceptions;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;
using DecoEnc.Utils;

namespace DecoEnc.Objects
{
    public struct KeyCryptor
    {
        public string Password { get; }

        public KeyCryptor(string key)
        {
            Password = key;
        }

        public void Zip()
        {
            if (!Directory.Exists("keys")) Directory.CreateDirectory("keys");
            var nameIni = $"key_{DateTime.Now.ToString().ToString().Replace(":", ".")}";
            var cacheDirectory = $"keys/.cache_{DateTime.Now.ToString().Replace(":", ".")}";
            Directory.CreateDirectory(cacheDirectory);
            var ini = IniConvert.Create($"{cacheDirectory}/{nameIni}");
            var cryptSettings = CryptSettings.Instance.Value;
            ini.Write("salt", cryptSettings.Salt, "CryptoSettings");
            ini.Write("key", Password, "CryptoKeys");
            ZipFile.CreateFromDirectory(cacheDirectory, $"keys/key_{DateTime.Now.ToString().Replace(":", ".")}.zip");
            Directory.Delete(cacheDirectory, true);
        }
    }
}
