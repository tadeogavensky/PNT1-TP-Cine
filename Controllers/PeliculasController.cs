using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNT1_TP_Cine.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PNT1_TP_Cine.Controllers
{
    public class PeliculasController : Controller
    {
        Context context = new Context();
        public IActionResult Index()
        {
            // Lista todas las peliculas con su genero
            var peliculas = context.Peliculas
                .Include(p => p.Genero) 
                .ToList();

            // Guarda la lista de peliculas en ViewBag
            ViewBag.Peliculas = peliculas;

            // Devuelve la vista con las peliculas
            return View(peliculas);
        }
        public IActionResult Detalle(string titulo)
        {
            // Verifica si el título de la película es nulo o vacío
            if (string.IsNullOrEmpty(titulo))
            {
                return NotFound("El título de la película no puede estar vacío.");
            }

            // Busca la película por título 
            var pelicula = context.Peliculas
                 .Include(p => p.Genero) 
                 .FirstOrDefault(p => p.Titulo == titulo);

            // Si no existe, devuelve error
            if (pelicula == null)
            {
                return NotFound($"No se encontró la película con el título: {titulo}.");
            }

            // Obtiene las funciones asociadas a la película
            List<Funcion> funciones = context.Funciones
                .Include(f => f.Sala)
                .Where(f => f.PeliculaId == pelicula.Id)
                .ToList();

            // ViewBags con los datos
            ViewBag.Funciones = funciones;
            ViewBag.Pelicula = pelicula;

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            // Devuelve la vista
            return View(pelicula);

        }


    }

}
