using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyManager
{
    public class KeyManager
    {
        public static byte[] GenerateKey()
        {
            return AesCryptoServiceProvider.Create().Key;
        }

        public static byte[] Encrypt(byte[] plainBytes, X509Certificate2 cert)
        {
            RSACryptoServiceProvider publicKey = (RSACryptoServiceProvider)cert.PublicKey.Key;
            publicKey.PersistKeyInCsp = false;

            // byte[] plainBytes = Convert.FromBase64String(plainText);
            byte[] encryptedBytes = publicKey.Encrypt(plainBytes, false);
            //string encryptedText = Convert.ToBase64String(encryptedBytes);
            //return encryptedText;
            return encryptedBytes;
        }

        public static string Decrypt(byte[] encryptedBytes, X509Certificate2 cert)
        {
            RSACryptoServiceProvider privateKey = (RSACryptoServiceProvider)cert.PrivateKey;
            //byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes = privateKey.Decrypt(encryptedBytes, false);
            string decryptedText = ASCIIEncoding.ASCII.GetString(decryptedBytes);
            return decryptedText;
        }

    }
}
