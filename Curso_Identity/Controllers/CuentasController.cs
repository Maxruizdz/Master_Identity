using Curso_Identity.Models;
using Curso_Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Curso_Identity.Controllers
{
    public class CuentasController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registro() { 
        RegistroViewModel registroVM= new RegistroViewModel();
        return View(registroVM);
        }
        [HttpPost]

        public async Task<IActionResult> Registro(RegistroViewModel vmRegistro) {

            if (ModelState.IsValid) { 
            
             var
                app_new_Usuari = new AppUsuario() {UserName= vmRegistro.Email,Nombre= vmRegistro.Nombre, Email= vmRegistro.Email, Ciudad= vmRegistro.Ciudad, CodigoPais=vmRegistro.CodigoPais, Pais= vmRegistro.Pais,Url= vmRegistro.Url , Direccion=vmRegistro.Direccion, FechaNacimiento= vmRegistro.FechaNacimiento, Estado= vmRegistro.Estado };
             var resultado=await _userManager.CreateAsync(app_new_Usuari, vmRegistro.Password);

                if (resultado.Succeeded)
                {

                    await _signInManager.SignInAsync(app_new_Usuari, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                else { ValidarErrores(resultado); }
            }


            return View(vmRegistro);
        }

        private void ValidarErrores(IdentityResult resultado)
        {

            foreach (var error in resultado.Errors) {

                ModelState.AddModelError(string.Empty, error.Description);
            
            
            }
        }
    }
}
