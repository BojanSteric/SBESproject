using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    [ServiceContract]
    public interface IReplikator
    {

        [OperationContract]
        void SendData(byte[] data, byte[] signature);

        [OperationContract]
        void Archive(byte[] data, byte[] signature);

        [OperationContract]
        void SendKey(byte[] key);
    }

}
