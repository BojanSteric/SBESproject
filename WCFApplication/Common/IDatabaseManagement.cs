﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    [ServiceContract]
    public interface IDatabaseManagement
    {
        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        void createDatabase(string fileName);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        void removeData(int id);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        void removeDatabase(string filename);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        void archivateDatabase(string fileName);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        void addData(int id, string region, string cityName, int year, double electricalEnergy);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        void modifyData(int id, City city);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        double averageForRegion(string region);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        double averageForCity(string cityName);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        List<City> maxConsumerForRegion(string region);


    }
}
