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

namespace Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            string cert = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            //Debugger.Launch();

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
            
            Console.WriteLine("henlo servis pokrenut");
            Console.ReadKey();
            host.Close();
        }
    }
}
