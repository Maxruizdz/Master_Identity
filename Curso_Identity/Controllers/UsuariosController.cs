using Curso_Identity.Datos;
using Curso_Identity.Models;
using Curso_Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Curso_Identity.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;


        public UsuariosController(UserManager<IdentityUser> userManager, ApplicationDbContext application) 
        { 
        
        _context = application;
            _userManager = userManager;
        
        
        }

        [HttpGet]
        public  async Task<IActionResult> Index()
        {

            var usuarios= await _context.AppUsuario.ToListAsync();
            var Roles_usuario = await _context.UserRoles.ToListAsync();
            var roles = await _context.Roles.ToListAsync();


            foreach (var usuario in usuarios) 
            {

                var rol = Roles_usuario.FirstOrDefault(p => p.UserId == usuario.Id);

                if (rol is null)
                {

                    usuario.Rol = "Ninguno";

                }
                else {

                    usuario.Rol = roles.FirstOrDefault(p => p.Id == rol.RoleId).Name;
                
                }

            
            }
            return View(usuarios);
        }


        [HttpGet]
        public IActionResult Editar(string id)
        {
          var usuario = _context.AppUsuario.Find(id);
          
            if (usuario == null)
            {

                TempData["Error"] = "El usuario no existe";
                return RedirectToAction(nameof(Index));


            }

            var rolUsuario = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();
            var rol = rolUsuario.FirstOrDefault(u => u.UserId == usuario.Id);
            if (rol != null) {


                usuario.IdRol = roles.FirstOrDefault(role => role.Id == rol.RoleId).Id;
            
            }
            usuario.ListaRoles = _context.Roles.Select(u=>new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem 
            { 
            Text= u.Name, 
             Value=u.Id
            
            });
            return View(usuario);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(CambiarPasswordViewModel passVm, string email) {


            if (ModelState.IsValid) 
            {

                var usuario = await _userManager.FindByEmailAsync(email);

                if (usuario is null) 
                {

                    return RedirectToAction("Error");
                
                
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);



                var resultado = await _userManager.ResetPasswordAsync(usuario,token,passVm.Password);

                if (resultado.Succeeded)
                {

                    return RedirectToAction("ConfirmacionCambioPassword");
                }
                else {

                    return View(passVm);
                }
            
            }


            return View(passVm);
        
        }

        [HttpGet]
        public IActionResult ConfirmacionCambioPassword() {

            return View();
        
        }

        }
    }

