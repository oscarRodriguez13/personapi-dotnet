// Estudio.cs
using System;
using System.Collections.Generic;
namespace personapi_dotnet.Models.Entities
{
    public class Estudio
    {
        public int IdProf { get; set; }
        public int CcPer { get; set; }
        public DateTime? Fecha { get; set; }
        public string Univer { get; set; }
        public virtual Persona? Persona { get; set; }
        public virtual Profesion? Profesion { get; set; }
        // Propiedades originales para EF Core
        public virtual Persona? CcPerNavigation { get => Persona; set => Persona = value; }
        public virtual Profesion? IdProfNavigation { get => Profesion; set => Profesion = value; }
    }
}