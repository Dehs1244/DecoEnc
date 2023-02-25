using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecoEnc.Objects.Exceptions;

namespace DecoEnc.Objects
{
    class CryptFile
    {
        private string _key;
        private IList<(string Base64, string FileName, string Extension)> _fileBase64 = new List<(string Base64, string FileName, string Extension)>();
        public int Count { get => _fileBase64.Count; }


        public CryptFile(string key, string path)
        {
            if (!File.Exists(path))
            {
                var files = Directory.EnumerateFiles(Path.GetDirectoryName(path));
                foreach(var file in files)
                {
                    _fileBase64.Add((Convert.ToBase64String((File.ReadAllBytes(file))), Path.GetFileNameWithoutExtension(file), Path.GetExtension(file)));
                }
            }
            else
            {
                _fileBase64.Add((Convert.ToBase64String((File.ReadAllBytes(path))), Path.GetFileNameWithoutExtension(path), Path.GetExtension(path)));
            }
            _key = key;
        }

        public CryptFileResult Crypt()
        {
            Cryptor crypt = new Cryptor(_key);
            List<CryptFileResult.CryptedFile> cryptedFiles = new List<CryptFileResult.CryptedFile>();
            foreach(var file in _fileBase64)
            {
                var cryptedInfo = crypt.Crypt(file.Base64);
                var cryptedExtension = crypt.Crypt(file.Extension);
                var crypted = cryptedInfo.CryptedValue + "\n" + cryptedExtension.CryptedValue;
                cryptedFiles.Add(new CryptFileResult.CryptedFile() { Extension = file.Extension, FileName = file.FileName, Result = new CryptResult() { CryptedValue = crypted } });
            }
            return new CryptFileResult(cryptedFiles);
        }
    }
}
