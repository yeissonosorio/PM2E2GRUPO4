using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2E2GRUPO4.Models
{
    public class Lugaresget
    {
        public int id { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public string? descripcion { get; set; }
        public byte[] firma { get; set; }
        public byte[] audio { get; set; }
    }
}
