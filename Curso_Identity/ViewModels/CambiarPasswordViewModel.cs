using System.ComponentModel.DataAnnotations;

namespace Curso_Identity.ViewModels
{
    public class CambiarPasswordViewModel
    {


        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public  string Password { get; set; }

        [Required(ErrorMessage ="La confirmacion de contraseña es obligatoria")]
        [Compare("Password", ErrorMessage= "La contraseña y confirmacion de contraseña no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]

        public string ConfirmPassword { get; set;}










    }
}
