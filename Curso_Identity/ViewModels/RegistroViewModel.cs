using System.ComponentModel.DataAnnotations;

namespace Curso_Identity.ViewModels
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(50, ErrorMessage = "El {0} debe estar al menos caracteres de longitud", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmacion de contraseña es obligatoria")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [StringLength(50, ErrorMessage = "El {0} debe estar al menos caracteres de longitud", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = " Confirmar contraseña")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        public string Url { get; set; }
        public Int32 CodigoPais { get; set; }
        public string telefono { get; set; }
        [Required(ErrorMessage = "El pais es oblidatorio")]
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [Display(Name ="Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage ="El estado es obligatorio")]
        public bool Estado { get; set; }
    }

    }
