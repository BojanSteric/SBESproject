using DataBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceDBManager : IDatabaseManagement
    {
        public DataIO serializer = new DataIO();
        public static Dictionary<int, City> CitiesDB;

        #region Modifier functions
        public void addData(int id, string region, string cityName, int year, double electricalEnergy)
        {
            if (!CitiesDB.ContainsKey(id))  //ako ne postoji podatak sa id-em, dodaj ga i apdejtuj bazu (fajl)
            {
                CitiesDB.Add(id, new City(id, region, cityName, year, electricalEnergy));
                updateDatabase();
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException(String.Format("Vec postoji podatak sa id-em {0}", id)));
            }

        }

        //basically update object if it exists and update database, ovo moze bolje ako se stave parametri umesto celog objekta, ali radi posao
        public void modifyData(int id, City city)
        {
            if (CitiesDB.ContainsKey(id))
            {
                CitiesDB[id] = city;
                updateDatabase();
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException("Ne mozete da izmenite nesto sto ne postoji"));
            }
        }

        //obrisi podatak ako postoji i apdejtuj bazu
        public void removeData(int id)
        {
            if (CitiesDB.ContainsKey(id))
            {
                CitiesDB.Remove(id);
                updateDatabase();
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException("Ne postoji takav podatak u bazi"));
            }
        }
        #endregion

        #region Admin functions
        public void archivateDatabase(string fileName)
        {
            string archiveFile = String.Format("{0} {1}.txt", fileName, DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss"));  //formira string za arhivni fajl

            if (!Directory.Exists("Archive"))   //ako ne postoji archive folder, napravi ga
            {
                Directory.CreateDirectory("Archive");
            }

            if (File.Exists(fileName + ".txt"))     //ako postoji fajl koji zelimo da arhiviramo
            {
                File.Copy(fileName + ".txt", Directory.GetCurrentDirectory() + "\\Archive\\" + archiveFile);    /*kopiraj zeljeni fajl u fajl za arhivnim 
                                                                                                                 * imenom i smesti ga u archive folder*/
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException("ne mozete arhivirati fajl koji ne postoji"));
            }
        }

        //Ako fajl ne postoji kreiramo ga, u suprotnom no-no
        public void createDatabase(string fileName)
        {
            if (!File.Exists(fileName + ".txt"))
            {
                File.Create(fileName + ".txt");
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException(String.Format("baza sa imenom \"{0}\" vec postoji", fileName)));
            }
        }

        //Obrisi bazu ako postoji, ako ne postoji baci exception
        public void removeDatabase(string filename)
        {
            if (File.Exists(filename + ".txt"))
            {
                File.Delete(filename + ".txt");
            }
            else
            {
                throw new FaultException<DatabaseException>(new DatabaseException("Ne postoji takav fajl u bazi"));
            }
        }
        #endregion


        #region Reader functions
        public double averageForCity(string cityName)
        {
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
                throw new FaultException<DatabaseException>(new DatabaseException("U bazi ne postoji taj grad"));
            }

            return result;
        }

        //averageForRegion je prakticno ista funkcija kao averageForCity, samo sto uzima regiju kao parametar
        public double averageForRegion(string region)
        {
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
                throw new FaultException<DatabaseException>(new DatabaseException("U bazi ne postoji ta regija"));
            }
            else
            {
                result /= counter;
            }

            return result;
        }

        public List<City> maxConsumerForRegion(string region)
        {
            List<City> max = new List<City>();

            try
            {
                double maximumForRegion = CitiesDB.Values.Where(c => c.Region.Equals(region.ToLower())).Max(c => c.ElectricalEnergy);

                foreach (City city in CitiesDB.Values.Where(c => c.Region.Equals( region.ToLower() ) ) )  //ovaj lambda izraz je samo filter za regiju, skracuje listu
                {
                    if (city.ElectricalEnergy == maximumForRegion)
                    {
                        max.Add(city);
                    }
                }
            }
            catch (Exception e)     //ovde ulazimo ako pukne foreach, a puca ako Where( lambda izraz ) nadje 0 objekata
            {
                throw new FaultException<DatabaseException>(new DatabaseException("U bazi ne postoji ta regija za max"));
            }

            return max;
        }

        #endregion


        //slobodna funkcija za inicijalizaciju dictionary-a na serveru
        public void loadDb()
        {
            CitiesDB = serializer.DeserializeFromTxt("cities.txt");
            if (CitiesDB == null)
            {
                CitiesDB = new Dictionary<int, City>();
            }
        }

        //slobodna funkcija za apdejt fajla baze
        public void updateDatabase()
        {
            serializer.SerializeToTxt(CitiesDB, "cities.txt");
        }
    }
}
