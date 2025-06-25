using Microsoft.AspNetCore.Mvc;
using PNT1_TP_Cine.Models;

namespace PNT1_TP_Cine.Controllers
{
    public class HomeController : Controller
    {

        Context context = new Context();

        public IActionResult Index()
        {
            // Lista todas las peliculas
            List<Pelicula> peliculas = context.Peliculas.ToList();

            // Guarda la lista de peliculas en ViewBag para que esté disponible en la vista
            ViewBag.Peliculas = peliculas;

            // Devuelve la vista principal
            return View();
        }
    }


}
