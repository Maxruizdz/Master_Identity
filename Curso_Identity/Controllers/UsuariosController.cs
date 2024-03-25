﻿using Curso_Identity.Datos;
using Curso_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Curso_Identity.Controllers
{
    public class UsuariosController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;


        public UsuariosController(UserManager<IdentityUser> userManager, ApplicationDbContext application) 
        { 
        
        _context = application;
            _userManager = userManager;
        
        
        }
        public IActionResult Index()
        {
            return View();
        }

  
        [HttpGet]
        public IActionResult EditarPerfil(string id)
        {

            if (id is null)
            {

                return NotFound();
            }

            var user = _context.AppUsuario.Find(id);
            if (user == null)
            {

                return NotFound();

            }


            return View(user);

        }

        [HttpPost]

        public async Task<IActionResult> EditarPerfil(AppUsuario appusuario) {



            if (ModelState.IsValid) {

      var usuario= await _context.AppUsuario.FindAsync(appusuario.Id);


                usuario.Nombre = appusuario.Nombre;
                usuario.telefono= appusuario.telefono;
                usuario.Url = appusuario.Url;
                usuario.UserName= appusuario.UserName;
                usuario.CodigoPais = appusuario.CodigoPais;
                usuario.Pais= appusuario.Pais;
                usuario.Direccion = appusuario.Direccion;
                usuario.FechaNacimiento = appusuario.FechaNacimiento;
            


                await _userManager.UpdateAsync(usuario);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), "Home");
            
                   
            }

            return View(appusuario);
        }

        [HttpGet]
        public IActionResult CambiarPassword() {

            return View();
        
        }


        }
    }

