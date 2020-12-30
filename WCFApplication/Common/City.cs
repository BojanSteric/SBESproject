using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    [DataContract]
    public class City
    {
        private int id;
        private string region;
        private string cityName;
        private int year;
        private double electricalEnergy;

        public City(int id, string region, string cityName, int year, double electricalEnergy)
        {
            Id = id;
            Region = region;
            CityName = cityName;
            Year = year;
            ElectricalEnergy = electricalEnergy;
        }
        public City()
        {
            electricalEnergy = -1;  //ovo setujem zbog trazenj
        }

        [DataMember]
        public int Id
        {
            get { return id; }
            set { this.id = value; }
        }

        [DataMember]
        public string Region
        {
            get { return region; }
            set { this.region = value; }
        }

        [DataMember]
        public string CityName
        {
            get { return cityName; }
            set { this.cityName = value; }
        }

        [DataMember]
        public int Year
        {
            get { return year; }
            set { this.year = value; }
        }

        [DataMember]
        public double ElectricalEnergy
        {
            get { return electricalEnergy; }
            set { this.electricalEnergy = value; }
        }
    }
}
