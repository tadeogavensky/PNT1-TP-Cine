using Microsoft.AspNetCore.Mvc;
using PNT1_TP_Cine.Models;

namespace PNT1_TP_Cine.Controllers
{
    public class HomeController : Controller
    {

        Context context = new Context();

        public IActionResult Index()
        {
            List<Pelicula> peliculas = context.Peliculas.ToList();

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
