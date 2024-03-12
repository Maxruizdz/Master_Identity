using System.ComponentModel.DataAnnotations;

namespace Curso_Identity.ViewModels
{
    public class OlvidoPasswordViewModel
    {

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
