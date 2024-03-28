using Curso_Identity.Datos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Curso_Identity.Controllers
{
    [Authorize(Roles ="Administrador")] 
    
    public class RolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public RolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {


            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;


        }

        [HttpGet]
        public IActionResult Index()
        {
            var roles = _context.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Crear()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(IdentityRole nuevo_rol) 
        {
            if (await _roleManager.RoleExistsAsync(nuevo_rol.Name)) 
            {
                TempData["Error"] = "El rol ya existe";
                return RedirectToAction(nameof(Index));
            }

            await _roleManager.CreateAsync(nuevo_rol);
            TempData["Correcto"] = "Rol creado correctamente";
            return RedirectToAction(nameof(Index));



        }
        [HttpGet]
        public IActionResult Editar(string id)
        {


            if (string.IsNullOrEmpty(id))
            {

                return View();
            }
            else {

             var rol=   _context.Roles.FirstOrDefault(rol=>rol.Id ==id);
              return View(rol);
            
            }

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(IdentityRole rol_modificado)
        {

            if (await _roleManager.RoleExistsAsync(rol_modificado.Name))
            {
                TempData["Error"] = "El rol ya existe";
                return RedirectToAction(nameof(Index));
            }
            var rol = _context.Roles.FirstOrDefault(rol => rol.Id == rol_modificado.Id);
            if (rol is null) 
            {


                return RedirectToAction(nameof(Index));

            }

            
            rol.Name = rol_modificado.Name;
            rol.NormalizedName =rol_modificado.Name.ToUpper();
            await _roleManager.UpdateAsync(rol);
            await _context.SaveChangesAsync();
            TempData["Correcto"] = "Rol editado correctamente";
            return RedirectToAction(nameof(Index));

            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrar(string id ) 
        { 
        var rol= _context.Roles.FirstOrDefault(r=>r.Id == id);
        if (rol is null)
            {

                TempData["Error"] = "No existe el rol";
                return RedirectToAction(nameof(Index));

            }

            var usuario_conRol = _context.UserRoles.Where(rol => rol.RoleId == id).Count();
            if (usuario_conRol>0) 
            {

                TempData["Error"] = "El rol tiene usuarios, no se puede borrar";
                return RedirectToAction(nameof(Index));


            }


            await _roleManager.DeleteAsync(rol);
            await _context.SaveChangesAsync();

            TempData["Correcto"] = "El rol ha sido eliminado exitosamente";

            return RedirectToAction(nameof(Index));

        }




    }
}
