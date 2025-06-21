using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNT1_TP_Cine.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PNT1_TP_Cine.Controllers
{
    public class PeliculasController : Controller
    {
        Context context = new Context();

       
        public IActionResult Detalle(string titulo)
        {
            if (string.IsNullOrEmpty(titulo))
            {
                return NotFound("El título de la película no puede estar vacío.");
            }
            var pelicula = context.Peliculas
                 .Include(p => p.Genero) 
                 .FirstOrDefault(p => p.Titulo == titulo);

            if (pelicula == null)
            {
                return NotFound($"No se encontró la película con el título: {titulo}.");
            }

            List<Funcion> funciones = context.Funciones
                .Include(f => f.Sala)
                .Where(f => f.PeliculaId == pelicula.Id)
                .ToList();

            ViewBag.Funciones = funciones;
            ViewBag.Pelicula = pelicula;

            return View(pelicula);


        }


    }

}
