using Microsoft.AspNetCore.Identity;
using System;

namespace Curso_Identity.Models
{
    public class AppUsuario : IdentityUser
    {
        public string Nombre { get; set; }
        public string Url { get; set; }
        public Int32 CodigoPais { get; set; }
        public string telefono { get; set; }
        public string Pais {get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool Estado{ get; set; }





    }
}
