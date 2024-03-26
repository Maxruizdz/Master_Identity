using Curso_Identity.Datos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Curso_Identity.Controllers
{
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
            
             return RedirectToAction(nameof(Index));
            }

            await _roleManager.CreateAsync(nuevo_rol);

            return RedirectToAction(nameof(Index));



        }
    }
}
