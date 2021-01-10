using CertificateManager;
using CryptographyManager;
using DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]

    public class ServiceDBManager : IDatabaseManagement
    {
        public DataIO serializer = new DataIO();
        public static Dictionary<int, City> CitiesDB = new Dictionary<int, City>();
        public string fileName = "";
        public static IReplikator proxy = Program.UspostaviVezuSaReplikatorom();
        public static string key = string.Empty;
        #region Modifier functions
        public void addData(int id, string region, string cityName, int year, double electricalEnergy, string uloga)
        {
            if (!(uloga.Equals("writers") || uloga.Equals("admins")))
            {
                Console.WriteLine("Client tried operation he does not have permission for.");
                throw new FaultException<DatabaseException>(new DatabaseException("No permission for that"));
                return;
            }

            if (!CitiesDB.ContainsKey(id))  //ako ne postoji podatak sa id-em, dodaj ga i apdejtuj bazu (fajl)
            {
                CitiesDB.Add(id, new City(id, region.ToLower(), cityName.ToLower(), year, electricalEnergy));
                updateDatabase(this.fileName);
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException(String.Format("Error: vec postoji podatak sa id-em {0}", id)));
            }

        }

        //basically update object if it exists and update database, ovo moze bolje ako se stave parametri umesto celog objekta, ali radi posao
        public void modifyData(int id, City city, string uloga)
        {
            if (!(uloga.Equals("writers") || uloga.Equals("admins")))
            {
                Console.WriteLine("Client tried operation he does not have permission for.");
                throw new FaultException<DatabaseException>(new DatabaseException("No permission for that"));
                return;
            }

            if (CitiesDB.ContainsKey(id))
            {
                CitiesDB[id] = city;
                updateDatabase(fileName);
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException(String.Format("Error: podatak sa id-em {0} ne postoji", id)));
            }
        }

        //obrisi podatak ako postoji i apdejtuj bazu
        public void removeData(int id, string uloga)
        {
            if ((uloga.Equals("writers") || uloga.Equals("admins")))
            {
                Console.WriteLine("Client tried operation he does not have permission for.");
                throw new FaultException<DatabaseException>(new DatabaseException("No permission for that"));
                return;
            }

            if (CitiesDB.ContainsKey(id))
            {
                CitiesDB.Remove(id);
                updateDatabase(fileName);
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException(String.Format("Error: podatak sa id-em {0} ne postoji", id)));
            }
        }
        #endregion

        #region Admin functions
        public void archivateDatabase(string fileName, string uloga)
        {
            if (!uloga.Equals("admins"))
            {
                Console.WriteLine("Client tried operation he does not have permission for.");
                throw new FaultException<DatabaseException>(new DatabaseException("No permission for that"));
                return;
            }

            string archiveFile = String.Format("{0} {1}.txt", fileName, DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss"));  //formira string za arhivni fajl

            if (!Directory.Exists("Archive"))   //ako ne postoji archive folder, napravi ga
            {
                Directory.CreateDirectory("Archive");
            }

            if (File.Exists(fileName))     //ako postoji fajl koji zelimo da arhiviramo
            {
                File.Copy(fileName, Directory.GetCurrentDirectory() + "\\Archive\\" + archiveFile);    /*kopiraj zeljeni fajl u fajl za arhivnim */
                //File.Copy(fileName, Directory.GetCurrentDirectory() + " - Copy\\Archive\\" + archiveFile);    /* imenom i smesti ga u archive folder*/
                Debugger.Launch();
                string signCertCn = Formatter.ParseName(WindowsIdentity.GetCurrent().Name).Substring(0, 3) + "S"; //CertCn + "S_sign"; // digitalni potpis od server je 'serS'
                X509Certificate2 signCer = CertificateManager.CertificateManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCn);
                byte[] sifrovano = CryptographyManager.EncryptDecryptManager.EncrypthFile(fileName, "ne radi upis", key);
                byte[] potpis = CryptographyManager.DigitalSignature.Create(sifrovano, signCer);
                proxy.Archive(sifrovano, potpis);
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException(String.Format("Error: fajl {0} ne postoji i ne moze se arhivirati", fileName)));
            }
        }

        //Ako fajl ne postoji kreiramo ga, u suprotnom no-no
        public void createDatabase(string fileName, string uloga)
        {
            if (!uloga.Equals("admins"))
            {
                Console.WriteLine("Client tried operation he does not have permission for.");
                throw new FaultException<DatabaseException>(new DatabaseException("No permission for that"));
                return;
            }

            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException(String.Format("Error: baza sa imenom \"{0}\" vec postoji", fileName)));
            }
        }

        //Obrisi bazu ako postoji, ako ne postoji baci exception
        public void removeDatabase(string filename, string uloga)
        {
            if (!uloga.Equals("admins"))
            {
                Console.WriteLine("Client tried operation he does not have permission for.");
                throw new FaultException<DatabaseException>(new DatabaseException("No permission for that"));
                return;
            }

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException(String.Format("Error: u bazi ne postoji fajl \"{0}\"", filename)));
            }
        }
        #endregion

        #region Reader functions
        public double averageForCity(string cityName, string uloga)
        {
            if ((uloga.Equals("writers") || uloga.Equals("admins") || uloga.Equals("readers")))
            {
                Console.WriteLine("Client tried operation he does not have permission for.");
                throw new FaultException<DatabaseException>(new DatabaseException("No permission for that"));
                return -1;
            }

            double result = 0;
            int counter = 0;
            foreach (City city in CitiesDB.Values)
            {
                if (city.CityName.Equals(cityName.ToLower()))   //nadji objekte sa odgovarajucim imenom, ToLower() da budemo non case sensitive tako je lakse
                {
                    result += city.ElectricalEnergy;
                    counter++;
                }
            }
            try
            {
                result /= counter;  //ako je counter 0 baca exception zbog deljenja sa 0, a realno counter == 0 jedino kad ne postoji trazeni grad
            }
            catch (Exception e)
            {
                throw new FaultException<DatabaseException>(new DatabaseException(String.Format("Error: U bazi ne postoji grad \"{0}\"", cityName)));
            }

            return result;
        }

        //averageForRegion je prakticno ista funkcija kao averageForCity, samo sto uzima regiju kao parametar
        public double averageForRegion(string region, string uloga)
        {
            if (!(uloga.Equals("writers") || uloga.Equals("admins") || uloga.Equals("readers")))
            {
                Console.WriteLine("Client tried operation he does not have permission for.");
                throw new FaultException<DatabaseException>(new DatabaseException("No permission for that"));
                return -1;
            }

            double result = 0;
            int counter = 0;

            foreach (City city in CitiesDB.Values)
            {
                if (city.Region.Equals(region.ToLower()))
                {
                    result += city.ElectricalEnergy;
                    counter++;
                }
            }
            if (counter == 0)
            {
                throw new FaultException<DatabaseException>(new DatabaseException(String.Format("Error: u bazi ne postoji regija \"{0}\"", region)));
            }
            else
            {
                result /= counter;
            }

            return result;
        }

        public List<City> maxConsumerForRegion(string region, string uloga)
        {

            List<City> max = new List<City>();
            if (!(uloga.Equals("writers") || uloga.Equals("admins") || uloga.Equals("readers")))
            {
                Console.WriteLine("Client tried operation he does not have permission for.");
                throw new FaultException<DatabaseException>(new DatabaseException("No permission for that"));
                return max;
            }
            try
            {
                double maximumForRegion = CitiesDB.Values.Where(c => c.Region.Equals(region.ToLower())).Max(c => c.ElectricalEnergy);

                foreach (City city in CitiesDB.Values.Where(c => c.Region.Equals(region.ToLower())))  //ovaj lambda izraz je samo filter za regiju, skracuje listu
                {
                    if (city.ElectricalEnergy == maximumForRegion)
                    {
                        max.Add(city);
                    }
                }
            }
            catch (Exception e)     //ovde ulazimo ako pukne foreach, a puca ako Where( lambda izraz ) nadje 0 objekata
            {
                throw new FaultException<DatabaseException>(new DatabaseException(String.Format("Error: u bazi ne postoji regija \"{0}\"", region)));
            }

            return max;
        }

        #endregion


        //deljena funkcija, svi mogu da je koriste da ucitaju trazenu bazu
        public bool loadDb(string fileName)
        {
            this.fileName = fileName;
            CitiesDB = new Dictionary<int, City>();

            if (File.Exists(fileName))
            {
                CitiesDB = serializer.DeserializeFromTxt(fileName);
                if (CitiesDB == null)
                {
                    CitiesDB = new Dictionary<int, City>();
                }
                return true;
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException("Ne posoji ta baza"));
            }
        }

        //slobodna funkcija za apdejt fajla baze
        public void updateDatabase(string fileName)
        {
            serializer.SerializeToTxt(CitiesDB, fileName);
        }


        //ucitavamo sve txt fajlove i saljemo ih klijentu
        public string[] loadAllDatabases()
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");  //svi txt fajlovi u debug folderu sa sve lokacijom

            for (int i = 0; i < files.Count(); i++)
            {           //iseci lokaciju fajla, samo pokazi ime i ekstenziju
                string[] temp = files[i].Split('\\');
                files[i] = temp.Last();
            }

            return files;
        }



        public void UploadDatabase(string token, Dictionary<int, City> baza)
        {
            foreach (KeyValuePair<int, City> kvp in baza)
            {
                CitiesDB[kvp.Key] = kvp.Value;      // u slucaju da postoji azurirace se, a ako ne postoji dodace se
            }
        }

        public Dictionary<int, City> DownloadDatabase(string token)
        {
            return CitiesDB;
        }
    }
}
