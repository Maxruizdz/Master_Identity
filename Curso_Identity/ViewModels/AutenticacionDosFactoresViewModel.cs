using System.ComponentModel.DataAnnotations;

namespace Curso_Identity.ViewModels
{
    public class AutenticacionDosFactoresViewModel
    {

        [Required]
        [Display(Name = "Codigo del autenticador")]
        public string Code { get; set; }

        public string Token { get; set; }


        public string UrlCodigoQr { get;set; }


    }
}
