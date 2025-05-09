using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace personapi_dotnet.Models.Entities
{
    public class Telefono
    {
        [Key]
        public string Num { get; set; }

        [Required(ErrorMessage = "El operador es obligatorio")]
        public string Oper { get; set; }

        [Required(ErrorMessage = "El dueño es obligatorio")]
        public int Duenio { get; set; }

        [ForeignKey("Duenio")]
        public virtual Persona DuenioNavigation { get; set; }
    }
}