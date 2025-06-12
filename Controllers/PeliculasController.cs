using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNT1_TP_Cine.Models;

namespace PNT1_TP_Cine.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly Context _context;

        public PeliculasController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var peliculas = _context.Peliculas.ToList(); 
            ViewBag.Peliculas = peliculas;
            return View();
        }


    }

}
