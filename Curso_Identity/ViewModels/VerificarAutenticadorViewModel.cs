using System.ComponentModel.DataAnnotations;

namespace Curso_Identity.ViewModels
{
    public class VerificarAutenticadorViewModel
    {
        [Required]
        [Display(Name = "Codigo del autenticador")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }


        [Display(Name = "Recordar datos?")]
        public bool Recordar { get; set; }
    }
}
