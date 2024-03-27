using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curso_Identity.Models
{
    public class AppUsuario : IdentityUser
    {
        public string Nombre { get; set; }
        public string Url { get; set; }
        public Int32 CodigoPais { get; set; }
        public string telefono { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool Estado { get; set; }

        [NotMapped]
        [Display(Name=("Rol para el usuario"))]
        public string  IdRol { get; set; }

        [NotMapped]
        public string Rol { get; set; }


        [NotMapped]
        public IEnumerable<SelectListItem> ListaRoles { get; set; }



    }
}
