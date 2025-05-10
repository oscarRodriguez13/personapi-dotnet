using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace personapi_dotnet.Models.Entities
{
    public class Telefono
    {
        public string Num { get; set; }
        public string Oper { get; set; }
        public int Duenio { get; set; }

        [JsonIgnore]
        public virtual Persona? DuenioNavigation { get; set; }
    }
}