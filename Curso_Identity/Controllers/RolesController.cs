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
        public IActionResult Index()
        {
            return View();
        }
    }
}
