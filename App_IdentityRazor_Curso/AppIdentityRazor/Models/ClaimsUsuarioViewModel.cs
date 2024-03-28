namespace AppIdentityRazor.Models
{
    public class ClaimsUsuarioViewModel
    {
        public string IdUsuario { get; set; }
        public ClaimsUsuarioViewModel()
        {
            Claims = new List<ClaimUsuario>();
        }

        public List<ClaimUsuario> Claims { get; set; }
        public class ClaimUsuario {
        
        public string TipoClaim { get; set; }

        public bool Seleccionado { get; set; }
        
        
        
        }
    }
}
