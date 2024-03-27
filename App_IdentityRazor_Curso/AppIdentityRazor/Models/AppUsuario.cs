using Microsoft.AspNetCore.Identity;

namespace AppIdentityRazor.Models
{
    public class AppUsuario : IdentityUser
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Pais { get; set; }

        public string Ciudad { get; set; }
    }
}
