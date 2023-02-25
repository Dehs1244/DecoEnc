using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DecoEnc.Objects
{
    class DecryptFileResult
    {
        public struct DecryptedFile
        {
            public string FileName;
            public byte[] FileData;
            public string Extension;
        }

        public IEnumerable<DecryptedFile> Results = Enumerable.Empty<DecryptedFile>();

        public DecryptedFile OnlyFile { get => Results.First(); }

        public DecryptFileResult(IEnumerable<DecryptedFile> crypted)
        {
            Results = crypted;
        }

        public void Write(string path)
        {
            var currentPath = Path.GetDirectoryName(path);
            foreach(var file in Results)
            {
                File.Create(currentPath + $"/{file.FileName}{file.Extension}").Close();
                File.WriteAllBytes(currentPath + $"/{file.FileName}{file.Extension}", file.FileData);
            }
        }
    }
}
