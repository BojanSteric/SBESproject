using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
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
                //U app.config-u su podeseni endpointi

                ChannelFactory<IDatabaseManagement> servicePrimary = new ChannelFactory<IDatabaseManagement>("ServiceA");
                ChannelFactory<IDatabaseManagement> serviceSecondary = new ChannelFactory<IDatabaseManagement>("ServiceB");


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
                                    baza = databasePrimary.DownloadDatabase("1");
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
                        Thread.Sleep(2000);
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
