using Curso_Identity.Models;
using Curso_Identity.Services;
using Curso_Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Curso_Identity.Controllers
{
    public class CuentasController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _EmailSender;
        public CuentasController(UserManager<IdentityUser> userManager, IEmailSender mailJetEmailSender, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _EmailSender = mailJetEmailSender;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registro(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            RegistroViewModel registroVM = new RegistroViewModel();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Registro(RegistroViewModel vmRegistro, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {

                var app_new_Usuari = new AppUsuario() { UserName = vmRegistro.Email, Nombre = vmRegistro.Nombre, Email = vmRegistro.Email, Ciudad = vmRegistro.Ciudad, CodigoPais = vmRegistro.CodigoPais, Pais = vmRegistro.Pais, Url = vmRegistro.Url, Direccion = vmRegistro.Direccion, FechaNacimiento = vmRegistro.FechaNacimiento, Estado = vmRegistro.Estado };
                var resultado = await _userManager.CreateAsync(app_new_Usuari, vmRegistro.Password);

                if (resultado.Succeeded)
                {
                    
                  var code = await _userManager.GenerateEmailConfirmationTokenAsync(app_new_Usuari);
                    var urlRetorno = Url.Action("ConfirmarEmail", "Cuentas", new { UserId = app_new_Usuari.Id, code = code }, protocol: HttpContext.Request.Scheme);

                    await _signInManager.SignInAsync(app_new_Usuari, isPersistent: false);

                    await _EmailSender.SendEmailAsync(vmRegistro.Email, "Confirmar su cuenta - Proyecto Identity", "Por favor confirme su contraseña dando click aqui <a href=\"" + urlRetorno + "\">enlace</a>");
                    return LocalRedirect(returnurl);
                }
                else { ValidarErrores(resultado); }
            }


            return View(vmRegistro);
        }

        private void ValidarErrores(IdentityResult resultado)
        {

            foreach (var error in resultado.Errors)
            {

                ModelState.AddModelError(string.Empty, error.Description);


            }
        }
        [HttpGet]

        public IActionResult Acceso(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            AccesoViewModel accesoViewModel = new AccesoViewModel();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Acceso(AccesoViewModel vmAcceso, string returnurl = null)
        {

            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var usuario = await _signInManager.PasswordSignInAsync(vmAcceso.Email, vmAcceso.Password, vmAcceso.RememberMe, lockoutOnFailure: true);

                if (usuario.Succeeded)
                {

                    return LocalRedirect(returnurl);
                }
                else if (usuario.IsLockedOut is true)
                {

                    return View("Bloqueado");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Acceso Invalido");

                    return View(vmAcceso);
                }

            }


            return View(vmAcceso);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SalirAplicacion()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");



        }

        [HttpGet]
        public IActionResult OlvidoPassword()
        {




            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OlvidoPassword(OlvidoPasswordViewModel olvidoPasswordViewModel)
        {

            if (ModelState.IsValid)
            {

                var usuario = await _userManager.FindByEmailAsync(olvidoPasswordViewModel.Email);

                if (usuario == null)
                {
                    return RedirectToAction("ConfirmacionOlvidoPassword");

                }

                var codigo = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                var urlRetorno = Url.Action("ResetPassword", "Cuentas", new { UserId = usuario.Id, code = codigo }, protocol: HttpContext.Request.Scheme);

                await _EmailSender.SendEmailAsync(olvidoPasswordViewModel.Email, "Recuperar contraseña-Proyecto Identity",
                     "Por favor recupere su contraseña dando click aqui <a href=\"" + urlRetorno + "\">enlace</a>");


            }


            return RedirectToAction("ConfirmacionOlvidoPassword");

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmacionOlvidoPassword()
        {



            return View();
        }


        [HttpGet]
        
        public IActionResult ResetPassword(string code = null)
        {


            return code == null ? View("Error") : View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> ResetPassword(RecuperaPasswordViewModel rpViewModel) {

            if (ModelState.IsValid) { 
            
            var usuario= await _userManager.FindByEmailAsync(rpViewModel.Email);
                if (usuario is null) {

                    return RedirectToAction("ConfirmacionRecuperaPassword");
                
                }

                var resultado = await _userManager.ResetPasswordAsync(usuario, rpViewModel.Code, rpViewModel.Password);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("ConfirmacionRecuperaPassword");
                }

                ValidarErrores(resultado);
      
            
            }

            return View(rpViewModel);
        
        
        
        
        }

        [HttpGet]
        public IActionResult ConfirmacionRecuperaPassword() {


            return View();
        
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmacionEmail(string UserId, string code) {

            if (UserId is null || code is null) { 
            
            
            return View("Error"); 
            }
            var usuario= await _userManager.FindByIdAsync(UserId);
            if (usuario is null)
            {

                return View("Error");
            }
             var resultado = await _userManager.ConfirmEmailAsync(usuario, code); 
            

            return View(resultado.Succeeded ? "ConfirmarEmail": 
                "Error");
        
        }


    }
}