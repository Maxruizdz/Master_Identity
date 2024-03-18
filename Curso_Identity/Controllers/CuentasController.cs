using Curso_Identity.Models;
using Curso_Identity.Services;
using Curso_Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Crypto.Operators;
using System.Security.Claims;

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

        //public IActionResult Acceso(string returnurl = null)
        //{
        //    ViewData["ReturnUrl"] = returnurl;
        //    AccesoViewModel accesoViewModel = new AccesoViewModel();

        //    return View();
        //}


        [HttpPost]
//        [ValidateAntiForgeryToken]
///*
//        public async Task<IActionResult> Acceso(AccesoViewModel vmAcceso, string returnurl = null)
//        {

//            ViewData["ReturnUrl"] = returnurl;
//            returnurl = returnurl ?? Url.Content("~/");

//            if (ModelState.IsValid)
//            {
//                var usuario = await _signInManager.PasswordSignInAsync(vmAcceso.Email, vmAcceso.Password, vmAcceso.RememberMe, lockoutOnFailure: true);

//                if (usuario.Succeeded)
//                {

//                    return LocalRedirect(returnurl);
//                }
//                else if (usuario.IsLockedOut is true)
//                {

//                    return View("Bloqueado");

//                }
//                else
//                {
//                    ModelState.AddModelError(string.Empty, "Acceso Invalido");

//                    return View(vmAcceso);
//                }

//            }


//            return View(vmAcceso);
//        }
//*/

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
        public async Task<IActionResult> ConfirmarEmail(string UserId, string code) {

            if (UserId is null || code is null) { 
            
            
            return View("Error"); 
            }
            var usuario= await _userManager.FindByIdAsync(UserId);
            if (usuario is null)
            {

                return View("Error");
            }
             var resultado = await _userManager.ConfirmEmailAsync(usuario, code); 
            

            return View(resultado.Succeeded ? "ConfirmarEmail" : 
                "Error");
        
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public IActionResult AccesoExterno(string proveedor, string returnurl = null)
        {
            
            var urlRetorno = Url.Action("AccesoExternoCallback", "Cuentas", new { ReturnUrl = returnurl });
            var propiedades = _signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRetorno);

            return Challenge(propiedades, proveedor);

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AccesoExternoCallback(string returnurl = null, string error = null)
        {
            returnurl = returnurl ?? Url.Content("~/");
            ViewData["ReturnUrl"] = returnurl;
            if (error != null)
            {

                ModelState.AddModelError(string.Empty, $"Error en el acceso externo {error}");
                return View(nameof(Acceso));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info is null)
            {

                return RedirectToAction(nameof(Acceso));

            }
            var resultado = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (resultado.Succeeded)
            {

                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);

                return LocalRedirect(returnurl);

            }
            else
            {

                ViewData["ReturnUrl"] = returnurl;
                ViewData["NombreAMostrarProveedor"] = info.ProviderDisplayName;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);

                return View("ConfirmacionAccesoExterno", new ConfirmacionAccesoExterno { Email = email, Name = name });



            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ConfirmacionAccesoExterno(ConfirmacionAccesoExterno acceViewModel, string returnurl = null)
        {


            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {

                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {

                    return View("Error");

                }
                var usuario = new AppUsuario { UserName = acceViewModel.Email, Email = acceViewModel.Email, Nombre = acceViewModel.Name };
                var resultado = await _userManager.CreateAsync(usuario);
                if (resultado.Succeeded)
                {

                    resultado = await _userManager.AddLoginAsync(usuario, info);
                    if (resultado.Succeeded)
                    {

                        await _signInManager.SignInAsync(usuario, isPersistent: false);

                        await _signInManager.UpdateExternalAuthenticationTokensAsync(info);

                        return LocalRedirect(returnurl);

                    }


                }
                ValidarErrores(resultado);

            }
            ViewData["ReturnUrl"] = returnurl;

            return View(acceViewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Acceso(AccesoViewModel accViewModel, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(accViewModel.Email, accViewModel.Password, accViewModel.RememberMe, lockoutOnFailure: true);

                if (resultado.Succeeded)
                {
                    //return RedirectToAction("Index", "Home");
                    return LocalRedirect(returnurl);
                }
                if (resultado.IsLockedOut)
                {
                    return View("Bloqueado");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Acceso inválido");
                    return View(accViewModel);
                }
            }

            return View(accViewModel);
        }

        [HttpGet]
        public IActionResult Acceso(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }

        [HttpGet] 
        public async Task<IActionResult> ActivarAutenticador() 
        { 
        var usuario= await _userManager.GetUserAsync(User);
            
            await _userManager.ResetAuthenticatorKeyAsync(usuario);

            var token = await _userManager.GetAuthenticatorKeyAsync(usuario);


            var adModel = new AutenticacionDosFactoresViewModel() { Token = token };

            return View(adModel); 
        
        
        }

        [HttpPost]
        public async Task<IActionResult> ActivarAutenticacion(AutenticacionDosFactoresViewModel autenDosfacvm) {

            if (ModelState.IsValid) {

                var usuario = await _userManager.GetUserAsync(User);
                var suceeded = await _userManager.VerifyTwoFactorTokenAsync(usuario, _userManager.Options.Tokens.AuthenticatorTokenProvider, autenDosfacvm.Code);
                if (suceeded)
                {

                    await _userManager.SetTwoFactorEnabledAsync(usuario, true);

                }
                else {

                    ModelState.AddModelError("Verificar", "Su autenticacion de dos factores no ha sido validada");
                
                }

            
            
            
            }

            return RedirectToAction(nameof(ConfirmacionAutenticador));
        
        }
        [HttpGet]

        public IActionResult ConfirmacionAutenticador() {


            return View();
        
        
        }



    }
}