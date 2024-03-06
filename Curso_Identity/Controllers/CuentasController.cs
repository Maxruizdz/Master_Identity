using Curso_Identity.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Curso_Identity.Controllers
{
    public class CuentasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registro() { 
        RegistroViewModel registroVM= new RegistroViewModel();
        return View(registroVM);
        }
    }
}
