using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2E2GRUPO4.Config
{
    public class Clonfig
    {
        public static string IPAdress = "192.169.0.9:8000";
        public static string RestApi = "api/Sitios";
        public static string PostEndpoint = "store";
        public static string GetEndpoint = "show";
        public static string UpEndpoint = "delete";
        public static string Post = string.Format("http://", IPAdress, RestApi, "/", PostEndpoint);
        public static string Get = string.Format("http://", IPAdress, RestApi, "/", GetEndpoint);
    }
}
