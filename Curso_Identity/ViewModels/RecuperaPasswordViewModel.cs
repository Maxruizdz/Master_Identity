using System.ComponentModel.DataAnnotations;

namespace Curso_Identity.ViewModels
{
    public class RecuperaPasswordViewModel
    {

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmacion de contraseña es obligatoria")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [StringLength(50, ErrorMessage = "El {0} debe estar al menos caracteres de longitud", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = " Confirmar contraseña")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
