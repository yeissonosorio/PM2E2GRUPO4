using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace PM2E2GRUPO4.Controllers
{
    public class controladorsitio
    {
        public async static Task<Models.Msg> CreateEmple(Models.lugares lugares)
        {
            var msg = new Models.Msg();

            String JsonObject = JsonConvert.SerializeObject(lugares);
            System.Net.Http.StringContent stringContent = new StringContent(JsonObject, Encoding.UTF8,
                "application/json");
            Console.WriteLine(stringContent);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMessage = null;
                    responseMessage = await client.PostAsync("http://127.0.0.1:8000/api/Sitios/store",stringContent);
                    if (responseMessage != null)
                    {
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            var resultado = responseMessage.Content.ReadAsStringAsync().Result;
                            msg = JsonConvert.DeserializeObject<Models.Msg>(resultado);
                        }
                    }
                    else
                    {
                        msg.Code = "01";
                        msg.message = "No hubo respuesta del servidor";
                        return msg;
                    }
                }
                return msg;
            }
            catch (Exception e)
            {
                msg.Code = "01";
                msg.message = "No hubo respuesta del servidor"+ e.Message;
                return msg;
            }
        }
    }
}
