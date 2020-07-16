using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace WebApplication1.Models
{
    public class RSA
    {
        public static string Rsa(string source)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString("<RSAKeyValue><Modulus>m6dgEA9rh6IYV/xlgJ1M2nCZthsxTBkGeq5eRcvdJLcSwescuwA7nKm4nCz8Nq2xDNfSVpyHwtuSNWOjxsg09p1ovsArb0b56M/0I7q0WTt4ojIFFdXOtw7CDyRevgl+QsHFAIK4m5OctkaP/4U9Peohr/DsnQroauyuNiYjE/0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
            var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(source), false);
            return Convert.ToBase64String(cipherbytes);
        }
        public static string UnRsa(string source, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            var cipherbytes = rsa.Decrypt(Convert.FromBase64String(source),false);
            return Encoding.UTF8.GetString(cipherbytes);
        }
    }
}