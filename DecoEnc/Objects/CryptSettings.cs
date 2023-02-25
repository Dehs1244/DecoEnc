using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecoEnc.Utils.Extensions;

namespace DecoEnc.Objects
{
    class CryptSettings
    {
        public int Iterations { get; private set; }
        public int KeySize { get; private set; }

        public string Hash { get; private set; }
        public string Salt { get; set; }
        public string Vector { get; private set; }

        public static Lazy<CryptSettings> Instance = new Lazy<CryptSettings>(new CryptSettings().GenerateBaseKey());

        public CryptSettings()
        {
            Iterations = 2;
            Salt = "qk3rqreqrkame";
            KeySize = 256;
            Hash = "SHA1";
            Vector = "8947az34awl34kjq";
        }

        public CryptSettings GenerateBaseKey()
        {
            Salt += "decenc";
            Random rand = new Random();
            Salt += rand.NextString();
            //Vector += rand.NextString();
            return this;
        }
    }
}
