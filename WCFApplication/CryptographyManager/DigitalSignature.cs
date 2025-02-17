﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyManager
{
    public class DigitalSignature
    {

        public static byte[] Create(byte[] dataToSign, X509Certificate2 certificate)
        {
            /// Looks for the certificate's private key to sign a message
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PrivateKey;

            if (csp == null)
            {
                throw new Exception("Valid certificate was not found.");
            }
            //UnicodeEncoding encoding = new UnicodeEncoding();
            //byte[] data = encoding.GetBytes(message);
            byte[] data = dataToSign;
            byte[] hash = null;

            SHA1Managed sha1 = new SHA1Managed();
            hash = sha1.ComputeHash(data);

            //SHA256Managed sha256 = new SHA256Managed();
            //hash = sha256.ComputeHash(data);

            /// Use RSACryptoServiceProvider support to create a signature using a previously created hash value
            byte[] signature = csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
            return signature;
        }


        public static bool Verify(byte[] dataToVerify, byte[] signature, X509Certificate2 certificate)
        {
            /// Looks for the certificate's public key to verify a message
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PublicKey.Key;

            //UnicodeEncoding encoding = new UnicodeEncoding();
            //byte[] data = encoding.GetBytes(message);
            byte[] data = dataToVerify;
            byte[] hash = null;

            SHA1Managed sha1 = new SHA1Managed();
            hash = sha1.ComputeHash(data);

            //SHA256Managed sha256 = new SHA256Managed();
            //hash = sha256.ComputeHash(data);

            /// Use RSACryptoServiceProvider support to compare two - hash value from signature and newly created hash value
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
        }

    }
}
