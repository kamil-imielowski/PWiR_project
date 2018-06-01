using System;
using System.Collections.Generic;
using System.ServiceModel;
using SharedLib;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //Uri adres = new Uri("http://localhost:2222/Test");
            ServiceHost host = new ServiceHost(typeof(DBManagment));
            //ServiceHost host = new ServiceHost(typeof(DBManagment), adres);
            //host.AddServiceEndpoint(typeof(IContract), new BasicHttpBinding(), adres);
            host.Open();
            Console.WriteLine("Serwer uruchomiony");
            Console.ReadKey();
        }
    }
}
