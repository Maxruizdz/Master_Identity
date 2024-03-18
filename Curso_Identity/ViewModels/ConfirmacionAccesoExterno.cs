using System.ComponentModel.DataAnnotations;

namespace Curso_Identity.ViewModels
{
    public class ConfirmacionAccesoExterno
    {
        [Required]
        [EmailAddress]

        public string Email { get; set; }

     [Required]

     public string Name{ get; set; }


    }
}
