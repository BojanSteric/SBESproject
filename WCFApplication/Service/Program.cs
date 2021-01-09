using DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CertificateManager;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using CryptographyManager;
using System.Threading;

namespace Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            string cert = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:9000/Service";
            
            //binding.Security.Mode = SecurityMode.Transport;
            //binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host = new ServiceHost(typeof(ServiceDBManager));
            host.AddServiceEndpoint(typeof(IDatabaseManagement), binding, address);

            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new Validator();
            
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            host.Credentials.ServiceCertificate.Certificate = CertificateManager.CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cert);

            host.Open();
            
            Console.WriteLine("hello servis pokrenut");

            IReplikator replikator = UspostaviVezuSaReplikatorom();

            string CertCn = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            string signCertCn = CertCn.Substring(0, 3) + "S"; //CertCn + "S_sign";
            X509Certificate2 cer = CertificateManager.CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, CertCn);
            X509Certificate2 signCer = CertificateManager.CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCn);

            byte[] key = KeyManager.GenerateKey();
            byte[] sifrovani_kljuc = KeyManager.Encrypt(key, cer);
            replikator.SendKey(sifrovani_kljuc);

            string infile = "../../zaReplikaciju.txt";
            string outFile = "../../kriptovanaReplikacija.txt";
            string kljuc = ASCIIEncoding.ASCII.GetString(key);
            ServiceDBManager.key = kljuc;
            DataIO serializer = new DataIO();
            while (serializer != null) // umesto while(true) ide ovaj uslov serializer != null da ne bi bacao warrning za mrtvi kod ispod while petlje
            {
                Dictionary<int, City> baza = ServiceDBManager.CitiesDB;
                if (baza.Count > 0) // ako ima podataka repliciraj
                {
                    Debugger.Launch();
                    serializer.SerializeToTxt(baza, infile);
                    byte[] sifrovano = CryptographyManager.EncryptDecryptManager.EncrypthFile(infile, outFile, kljuc);
                    byte[] potpis = CryptographyManager.DigitalSignature.Create(sifrovano, signCer);
                    replikator.SendData(sifrovano, potpis);
                }
                Thread.Sleep(7000); //replikacija ide na svakih 7 sekundi
            }

            Console.ReadKey();
            host.Close();
        }

        public static IReplikator UspostaviVezuSaReplikatorom()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string adresa = "net.tcp://localhost:9999/ReplikatorService";
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            EndpointAddress address = new EndpointAddress(new Uri(adresa));
            IReplikator Proxy;
            ChannelFactory<IReplikator> channel = new ChannelFactory<IReplikator>(binding, address);
            Proxy = channel.CreateChannel();

            return Proxy;
        }
    }
}
