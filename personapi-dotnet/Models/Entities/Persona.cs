using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace personapi_dotnet.Models.Entities
{
    public class Persona
    {
        public Persona()
        {
            Estudios = new HashSet<Estudio>();
            Telefonos = new HashSet<Telefono>();
        }

        public int Cc { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int? Edad { get; set; }
        public string Genero { get; set; }

        [JsonIgnore]
        public virtual ICollection<Estudio> Estudios { get; set; }

        [JsonIgnore]
        public virtual ICollection<Telefono> Telefonos { get; set; }
    }
}