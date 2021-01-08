using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CertificateManager;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            string servCert = "serverser";

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9000/Service";
            binding.Security.Mode = SecurityMode.Transport;

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            X509Certificate2 srvCert = CertificateManager.CertificateManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, servCert);
            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address), new X509CertificateEndpointIdentity(srvCert));

            Console.WriteLine("uspesno pokrenut");

            //nemam pojma sta je ovo EndpointIdentity.CreateUpnIdentity("wcfServer")
            //EndpointAddress endpointAddress = new EndpointAddress(new Uri(address)); 

            using (ClientProxy proxy = new ClientProxy(binding, endpointAddress, servCert))
            {
                //proxy.Credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, servCert);
                //proxy.Credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, servCert);
                Console.WriteLine("izaberite bazu koju zelite da koristite:");
                proxy.loadAllDatabases();       //ucitaj i ispisi sve fajlove koji postoje u bazi
                string uloga = proxy.Uloga;

                bool loaded = false;    //ovo je za ponavljanje odabira baze ako korisnik unese nesto sto ne postoji
                do
                {
                    loaded = proxy.loadDb(Console.ReadLine());
                } while (!loaded);



                //pozivi funkcija, cisto da vidimo dal rade
                proxy.addData(1011, "pomoravlje", "pozarevac", 1999, 23.3, uloga);
                Console.WriteLine();
                proxy.addData(222, "podunavlje", "smederevo", 1800, 10, uloga);
                Console.WriteLine();
                proxy.averageForCity("pozarevac", uloga);
                Console.WriteLine();
                proxy.averageForRegion("ndzamena", uloga);
                Console.WriteLine();
                proxy.maxConsumerForRegion("pomoravlje", uloga);
                Console.WriteLine();
                proxy.maxConsumerForRegion("podunavlje", uloga);
                Console.WriteLine();
                proxy.maxConsumerForRegion("asdasd", uloga);
                Console.WriteLine();
                proxy.removeData(2, uloga);
                Console.WriteLine();
                proxy.addData(1, "test", "lele", 2002, 33.1, uloga);
                Console.WriteLine();
                proxy.modifyData(1, new DataBase.City(1, "ziza", "kriza", 2020, 55.6), uloga);
                Console.WriteLine();
                proxy.removeData(15, uloga);
                Console.WriteLine();
                proxy.createDatabase("moj otac.txt", uloga);
                Console.WriteLine();
                //proxy.removeDatabase("moj otac");

                proxy.archivateDatabase("cities.txt", uloga);
                Console.WriteLine();
                proxy.archivateDatabase("asdasd", uloga);
            }
            
            Console.ReadLine();
        }
    }
}
