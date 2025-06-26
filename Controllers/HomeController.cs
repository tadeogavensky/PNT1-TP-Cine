using Microsoft.AspNetCore.Mvc;
using PNT1_TP_Cine.Models;

namespace PNT1_TP_Cine.Controllers
{
    public class HomeController : Controller
    {

        //Context context = new Context();
        private readonly Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Pelicula> peliculas = _context.Peliculas.ToList();

            ViewBag.Peliculas = peliculas;
            return View();
        }

        public IActionResult Nosotros()
        {
            return View();
        }

        public IActionResult Promociones()
        {
            return View();
        }
    }


}
