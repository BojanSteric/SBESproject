using DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Debugger.Launch();

            //NetTcpBinding binding = new NetTcpBinding();
            //string address = "net.tcp://localhost:8001/Service";
            
            //binding.Security.Mode = SecurityMode.Transport;
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host = new ServiceHost(typeof(ServiceDBManager));
            //host.AddServiceEndpoint(typeof(IDatabaseManagement), binding, address);

            host.Open();
            
            Console.WriteLine("henlo servis pokrenut");
            Console.ReadKey();
        }
    }
}
