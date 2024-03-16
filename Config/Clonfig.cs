using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2E2GRUPO4.Config
{
    public class Clonfig
    {
        public static string IPAdress = "https://ed79-170-83-119-110.ngrok-free.app";
        public static string RestApi = "/api/Sitios";
        public static string PostEndpoint = "store";
        public static string GetEndpoint = "show";
        public static string UpEndpoint = "delete";
        public static string Post = string.Format(IPAdress, RestApi, "/", PostEndpoint);
        public static string Get = string.Format(IPAdress, RestApi);
    }
}
