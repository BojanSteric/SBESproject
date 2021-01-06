using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Replikator
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {

                NetTcpBinding binding1 = new NetTcpBinding();
                NetTcpBinding binding2 = new NetTcpBinding();
                string address1 = "net.tcp://localhost:8001/Service";
                string address2 = "net.tcp://localhost:8002/Service";

                binding1.Security.Mode = SecurityMode.Transport;
                binding1.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                binding1.Security.Transport.ProtectionLevel =
                System.Net.Security.ProtectionLevel.EncryptAndSign;

                binding2.Security.Mode = SecurityMode.Transport;
                binding2.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                binding2.Security.Transport.ProtectionLevel =
                System.Net.Security.ProtectionLevel.EncryptAndSign;

                EndpointAddress endpointAddress1 = new EndpointAddress(new Uri(address1));
                EndpointAddress endpointAddress2 = new EndpointAddress(new Uri(address2));


                ChannelFactory <IDatabaseManagement> servicePrimary = new ChannelFactory<IDatabaseManagement>(binding1, endpointAddress1);
                ChannelFactory<IDatabaseManagement> serviceSecondary = new ChannelFactory<IDatabaseManagement>(binding2, endpointAddress2);


                while (true)
                {
                    try
                    {
                        //TODO: implementirati windows autentifikaciju

                        IDatabaseManagement databasePrimary = servicePrimary.CreateChannel();

                        IDatabaseManagement databaseSecondary = serviceSecondary.CreateChannel();

                        //TODO: ODREDITI KOJI JE PRIMARNI, PROVERAVATI STANJE
                        //EStanjeServera stanjeServisaA = EStanjeServera.Nepoznato;
                        //EStanjeServera stanjeServisaB = EStanjeServera.Nepoznato;
                        //try
                        //{
                        //    ChannelFactory<IStanjeServisa> stanjeA = new ChannelFactory<IStanjeServisa>("StanjeServisaA");
                        //    IStanjeServisa stanjeServisA = stanjeA.CreateChannel();
                        //    stanjeServisaA = stanjeServisA.ProveraStanja();
                        //}
                        //catch (FaultException<StanjeServisaIzuzetak> ex)
                        //{
                        //    Console.WriteLine($"Greska : {ex.Detail.Greska}");
                        //}
                        //catch (CommunicationException ex)
                        //{
                        //    Console.WriteLine($"Greska : {ex.Message}");
                        //}

                        //try
                        //{
                        //    ChannelFactory<IStanjeServisa> stanjeB = new ChannelFactory<IStanjeServisa>("StanjeServisaB");
                        //    IStanjeServisa stanjeServisB = stanjeB.CreateChannel();
                        //    stanjeServisaB = stanjeServisB.ProveraStanja();
                        //}
                        //catch (FaultException<StanjeServisaIzuzetak> ex)
                        //{
                        //    Console.WriteLine($"Greska : {ex.Detail.Greska}");
                        //}
                        //catch (CommunicationException ex)
                        //{
                        //    Console.WriteLine($"Greska : {ex.Message}");
                        //}

                        try
                        {
                            Dictionary<int, City> baza = new Dictionary<int, City>();

                            //OVDE ODRADITI PROVERU KOJI SERVIS JE PRIMARNI I NA OSNOVU TOGA DOWNLOAD/UPLOAD DATABASE
                            //if (!stanjeServisaA.Equals(EStanjeServera.Nepoznato) && !stanjeServisaA.Equals(EStanjeServera.Nepoznato))
                            //{
                            //    if (stanjeServisaA.Equals(EStanjeServera.Primarni))
                            //    {
                                    baza = databasePrimary.DownloadDatabase("token1");
                                    databaseSecondary.UploadDatabase("2", baza);
                                //}
                                //else if (stanjeServisaB.Equals(EStanjeServera.Primarni))
                                //{
                                //    baza = kanalBibliotekaB.PreuzmiBazu(tokenAdminB);
                                //    kanalBibliotekaA.PosaljiBazu(tokenAdminA, baza);
                                //}
                                Console.WriteLine("Replicirano je {0} podataka", baza.Count);
                            //}

                        }
                        catch (FaultException<DatabaseException> e)
                        {
                            Console.WriteLine(e.Detail.Message);
                        }
                        Thread.Sleep(3000);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}
