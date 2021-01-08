using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CertificateManager;
using System.ServiceModel.Security;

namespace Client
{
    //ovde zapravo pravimo klijentske funkcije
    public class ClientProxy : ChannelFactory<IDatabaseManagement>, IDatabaseManagement, IDisposable
    {
        IDatabaseManagement factory;

        public ClientProxy(NetTcpBinding binding, EndpointAddress address, string servCert) : base(binding, address)
        {
            // Credentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            this.Credentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, servCert);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new Validator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            factory = this.CreateChannel();
        }

        #region Admin functions
        public void archivateDatabase(string fileName, X509Certificate cer)
        {
            
            try
            {
                factory.archivateDatabase(fileName, cer);
                Console.WriteLine("Baza {0} uspesno arhivirana", fileName);
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine(e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void createDatabase(string fileName, X509Certificate cer)
        {
            
            try
            {
                factory.createDatabase(fileName, cer);
                Console.WriteLine("Baza uspesno kreirana");
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine(e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void removeDatabase(string filename, X509Certificate cer)
        {
            
            try
            {
                factory.removeDatabase(filename, cer);
                Console.WriteLine("Baza sa imenom \"{0}\" uspesno obrisana", filename);
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine(e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region Modifier function
        public void modifyData(int id, City city)
        {
            try
            {
                factory.modifyData(id, city);
                Console.WriteLine("Uspesno izmenjen grad sa id-em {0}", id);
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine(e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void removeData(int id)
        {
            try
            {
                factory.removeData(id);
                Console.WriteLine("Uspesno obrisan grad");
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine("Error: {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void addData(int id, string region, string cityName, int year, double electricalEnergy)
        {
            try
            {
                factory.addData(id, region, cityName, year, electricalEnergy);
                Console.WriteLine("Added city to database:\n\tid: {4}\n\tregion: {0}\n\tname: {1}\n\tyear: {2}\n\tconsumed energy: {3}", region, cityName, year, electricalEnergy, id);
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine(e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region Reader functions
        public double averageForCity(string cityName)
        {
            double result = 0;
            try
            {
                result = factory.averageForCity(cityName);
                Console.WriteLine("avg for city {0}: {1}", cityName, result);
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine("Error: {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        public double averageForRegion(string region)
        {
            double result = 0;
            try
            {
                result = factory.averageForRegion(region);
                Console.WriteLine("avg for region {0}: {1}", region, result);
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine("Error: {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        public List<City> maxConsumerForRegion(string region)
        {
            List<City> city = new List<City>();
            try
            {
                city = factory.maxConsumerForRegion(region);

                Console.WriteLine("Najveci konzumatori za regiju {0}:", region);
                foreach (City c in city) {
                    Console.WriteLine("\t{0} {1} {2} {3} {4}", c.Id, c.Region, c.CityName, c.Year, c.ElectricalEnergy);
                }
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine("Error: {0}", e.Detail.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return city;
        }

        #endregion

        public bool loadDb(string fileName)
        {
            try
            {
                factory.loadDb(fileName);
                return true;
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine(e.Detail.Message);
                return false;
            }
            catch(Exception e) 
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public string[] loadAllDatabases()
        {
            string[] files = new string[100];
            try
            {
                files = factory.loadAllDatabases();
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);  
            }
            
            return null;
        }

        //please dont implement this lmao rofl lol
        public void UploadDatabase(string token, Dictionary<int, City> baza)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, City> DownloadDatabase(string token)
        {
            throw new NotImplementedException();
        }
    }
}
