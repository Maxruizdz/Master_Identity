﻿using Curso_Identity.Models;
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
        public async Task<IActionResult> Registro(string returnurl=null) {
            ViewData["ReturnUrl"] = returnurl;
            RegistroViewModel registroVM= new RegistroViewModel();
        return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Registro(RegistroViewModel vmRegistro, string returnurl = null) {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid) { 
            
             var
                app_new_Usuari = new AppUsuario() {UserName= vmRegistro.Email,Nombre= vmRegistro.Nombre, Email= vmRegistro.Email, Ciudad= vmRegistro.Ciudad, CodigoPais=vmRegistro.CodigoPais, Pais= vmRegistro.Pais,Url= vmRegistro.Url , Direccion=vmRegistro.Direccion, FechaNacimiento= vmRegistro.FechaNacimiento, Estado= vmRegistro.Estado };
             var resultado=await _userManager.CreateAsync(app_new_Usuari, vmRegistro.Password);

                if (resultado.Succeeded)
                {

                    await _signInManager.SignInAsync(app_new_Usuari, isPersistent: false);

                    return LocalRedirect(returnurl);
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
        [HttpGet]
     
        public IActionResult Acceso(string returnurl = null) {
            ViewData["ReturnUrl"]= returnurl;
            returnurl = returnurl ?? Url.Content("~/");
        AccesoViewModel accesoViewModel= new AccesoViewModel();

        return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Acceso(AccesoViewModel vmAcceso, string returnurl=null)
        {

            ViewData["ReturnUrl"]= returnurl;


            if (ModelState.IsValid)
            {
                var usuario = await _signInManager.PasswordSignInAsync(vmAcceso.Email, vmAcceso.Password, vmAcceso.RememberMe, lockoutOnFailure:false);
            
                 if (usuario.Succeeded)
                    {

                    return LocalRedirect(returnurl);
                    }
                  else {ModelState.AddModelError(string.Empty, "Acceso Invalido");

                    return View(vmAcceso);
                }
                
            }


            return View(vmAcceso);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> SalirAplicacion() {

            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        
        
        
        }

    }
}
