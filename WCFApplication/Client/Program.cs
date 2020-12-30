using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Service";

            Console.WriteLine("uspesno pokrenut");

            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address), EndpointIdentity.CreateUpnIdentity("wcfServer")); //nemam pojma sta je ovo

            using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
            {
                //pozivi funkcija, cisto da vidimo dal rade
                proxy.addData(10, "pomoravlje", "pozarevac", 1999, 23.3);
                Console.WriteLine();
                proxy.addData(15, "podunavlje", "pozarevac", 1800, 10);
                Console.WriteLine();
                proxy.averageForCity("pozarevac");
                Console.WriteLine();
                proxy.averageForRegion("ndzamena");
                Console.WriteLine();
                proxy.maxConsumerForRegion("pomoravlje");
                Console.WriteLine();
                proxy.maxConsumerForRegion("podunavlje");
                Console.WriteLine();
                proxy.maxConsumerForRegion("asdasd");
                Console.WriteLine();
                proxy.removeData(2);
                Console.WriteLine();
                proxy.addData(1, "test", "lele", 2002, 33.1);
                Console.WriteLine();
                proxy.modifyData(1, new DataBase.City(1, "ziza", "kriza", 2020, 55.6));
                Console.WriteLine();
                proxy.removeData(15);
                Console.WriteLine();
                proxy.createDatabase("moj otac");
                Console.WriteLine();
                //proxy.removeDatabase("moj otac");
                proxy.archivateDatabase("cities");
                Console.WriteLine();
                proxy.archivateDatabase("asdasd");
            }
            
            Console.ReadLine();
        }
    }
}
