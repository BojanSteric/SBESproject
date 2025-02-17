﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DataBase
{
    //klasa za serijalizaciju i deserijalizaciju :(
    public class DataIO
    {
       //prosledis dictionary i fileName(sa ekstenzijom za sad)
        public  void SerializeToTxt(Dictionary<int, City> dictionary, string fileName)
        {
            //ako je dictionary prazan, baci skip
            if (dictionary == null) { return; }

            try
            {
                
                StreamWriter sw = new StreamWriter(fileName);

                //za svaki objekat u dictionary-u upisi u jednom redu sve propertije odvojene razmakom
                foreach (City city in dictionary.Values)
                {
                    sw.WriteLine(String.Format("{0} {1} {2} {3} {4}", city.Id, city.Region, city.CityName, city.Year, city.ElectricalEnergy));
                }

                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //vraca ucitan dictionary (temp), parametar je samo ime fajla sa ekstenzijom
        public  Dictionary<int, City> DeserializeFromTxt(string fileName)
        {
            Dictionary<int, City> temp = new Dictionary<int, City>();

            //ako je ime fajla prazan string, baci skip i vrati prazan dictionary
            if (string.IsNullOrEmpty(fileName)) { return temp; }

            try
            {
                StreamReader sr = new StreamReader(fileName);
                string line = sr.ReadLine();

                //citaj red po red, dok ne dodjes do praznog
                while (line != null)
                {
                    //parsiranje podataka: pocepaj red na svaki razmak, [0] (id) i [3] (godina) parsiraj kao int, [4] (energija) parsiraj kao double
                    string[] tmp = line.Split(' ');
                    int id = int.Parse(tmp[0]);
                    int year = int.Parse(tmp[3]);
                    double ee = double.Parse(tmp[4]);
                   
                    //napravi objekat i smesti ga u dictionary, objekat.Id == dictionary.Key
                    temp.Add(id, new City(id, tmp[1], tmp[2], year, ee));

                    //Predji na narednu liniju
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return temp;
        }
    }
}
