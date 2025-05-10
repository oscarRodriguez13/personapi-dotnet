using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace personapi_dotnet.Models.Entities
{
    public class Profesion
    {
        public Profesion()
        {
            Estudios = new HashSet<Estudio>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }
        public string Des { get; set; }

        [JsonIgnore]
        public virtual ICollection<Estudio> Estudios { get; set; }
    }
}