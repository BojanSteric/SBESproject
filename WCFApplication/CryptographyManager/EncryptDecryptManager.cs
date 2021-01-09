using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyManager
{
    public class EncryptDecryptManager
    {
        public static byte[] EncrypthFile(string inFile, string outFile, string key)
        {

            byte[] encrypthedBody = null;
            byte[] body = null;     //image body to be encrypted

            //body = File.ReadAllBytes(inFile);
            string s = File.ReadAllText(inFile);

            //body = Encoding.UTF8.GetBytes(s);
            body = ASCIIEncoding.ASCII.GetBytes(s);
            //body = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, body);

            AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(key),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            };

            aesCryptoServiceProvider.GenerateIV();
            ICryptoTransform cryptoTransform = aesCryptoServiceProvider.CreateEncryptor();

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                {

                    cs.Write(body, 0, body.Length);

                }
                encrypthedBody = aesCryptoServiceProvider.IV.Concat(ms.ToArray()).ToArray();
            }
            if (!outFile.Equals("ne radi upis"))
            {
                BinaryWriter bw = new BinaryWriter(File.OpenWrite(outFile));
                bw.Write(encrypthedBody);
                bw.Flush();
                bw.Close();
            }

            return encrypthedBody;
        }

        public static byte[] DecrypthFile(byte[] inFile, string outFile, string key)
        {
            byte[] decryptedBody = null;
            byte[] sifrovano = inFile;

            AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider()
            {
                Key = ASCIIEncoding.ASCII.GetBytes(key),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros,
            };


            aesCryptoServiceProvider.IV = sifrovano.Take(aesCryptoServiceProvider.BlockSize / 8).ToArray();
            ICryptoTransform cryptoTransform = aesCryptoServiceProvider.CreateDecryptor();

            using (MemoryStream ms = new MemoryStream(sifrovano.Skip(aesCryptoServiceProvider.BlockSize / 8).ToArray()))
            {
                using (CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Read))
                {

                    decryptedBody = new byte[sifrovano.Length - aesCryptoServiceProvider.BlockSize / 8];
                    cs.Read(decryptedBody, 0, decryptedBody.Length);

                }
            }

            return decryptedBody;

            //BinaryWriter bw = new BinaryWriter(File.OpenWrite(outFile));
            //bw.Write(decryptedBody);
            //bw.Flush();
            //bw.Close();
        }
    }
}
