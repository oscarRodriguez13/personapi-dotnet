// Estudio.cs
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace personapi_dotnet.Models.Entities
{
    public class Estudio
    {
        public int IdProf { get; set; }
        public int CcPer { get; set; }
        public DateTime? Fecha { get; set; }
        public string Univer { get; set; }

        [JsonIgnore]
        public virtual Persona? Persona { get; set; }

        [JsonIgnore]
        public virtual Profesion? Profesion { get; set; }
        // Propiedades originales para EF Core
        [JsonIgnore]
        public virtual Persona? CcPerNavigation { get => Persona; set => Persona = value; }
        [JsonIgnore]
        public virtual Profesion? IdProfNavigation { get => Profesion; set => Profesion = value; }
    }
}