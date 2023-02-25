using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoEnc.Objects
{
    class DecryptFile
    {
        private KeyCryptor _key;
        public CryptSettings Settings;
        private IList<(string CryptedInfo, string FileName)> _cryptedFiles = new List<(string CryptedInfo, string FileName)>();
        public int Count { get => _cryptedFiles.Count; }


        public DecryptFile(KeyCryptor key, string path)
        {
            if (!File.Exists(path))
            {
                var files = Directory.EnumerateFiles(Path.GetDirectoryName(path));
                foreach (var file in files)
                {
                    _cryptedFiles.Add((File.ReadAllText(file), file));
                }
            }
            else
            {
                _cryptedFiles.Add((File.ReadAllText(path), Path.GetFileNameWithoutExtension(path)));
            }
            _key = key;
        }

        public DecryptFileResult Decrypt()
        {
            Settings ??= CryptSettings.Instance.Value;
            Decryptor decrypt = new Decryptor(_key) { CryptSettings = Settings };
            List<DecryptFileResult.DecryptedFile> decryptedFiles = new List<DecryptFileResult.DecryptedFile>();
            foreach (var file in _cryptedFiles)
            {
                var splittedInfo = file.FileName.Split("\n");
                var fileInfo = splittedInfo[0];
                var extension = splittedInfo[1];
                var decryptedInfo = decrypt.Decrypt(fileInfo);
                var decryptedExtension = decrypt.Decrypt(extension);
                decryptedFiles.Add(new DecryptFileResult.DecryptedFile() { FileName = file.FileName, Extension = decryptedExtension.DecryptedValue.Replace("\0", string.Empty), FileData = Convert.FromBase64String(decryptedInfo.DecryptedValue.Replace("\0", string.Empty)) });
            }
            return new DecryptFileResult(decryptedFiles);
        }
    }
}
