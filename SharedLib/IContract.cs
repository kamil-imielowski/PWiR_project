using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;


namespace SharedLib
{
    [ServiceContract]
    public interface IContract
    {
        [OperationContract]
        List<Brand> GetBrands();

        [OperationContract]
        List<Phone> GetPhones();

        [OperationContract]
        bool AddBrand(string name);

        [OperationContract]
        bool AddPhone(Phone phone);
    }
}
