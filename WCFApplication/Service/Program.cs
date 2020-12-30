using DataBase;
using System;
using System.Collections.Generic;
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
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Service";

            //load database 
            ServiceDBManager sm = new ServiceDBManager();
            sm.loadDb();

            ServiceHost host = new ServiceHost(typeof(ServiceDBManager));
            host.AddServiceEndpoint(typeof(IDatabaseManagement), binding, address);

            host.Open();
            
            Console.WriteLine("henlo servis pokrenut");
            Console.ReadKey();
        }
    }
}
