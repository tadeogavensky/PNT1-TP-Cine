using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNT1_TP_Cine.Models;

namespace PNT1_TP_Cine.Controllers
{
    public class Panel : Controller
    {
        Context context = new Context();
        public IActionResult Admin()
        {
            List<Funcion> listaDeFunciones = context.Funciones.Include(f => f.Sala).ToList(); ; // Listo las funciones
            ViewBag.Funciones = listaDeFunciones; // Mando la lista de funciones a la Vista

            // Aca vamos a contar la cantidad de salas por cada funcion
            var listaAuxDeFunciones = context.Funciones.ToList(); // Trae funciones a memoria

            var salasConCantidad = context.Salas
                .AsEnumerable() // Trae salas a memoria para poder usar LINQ en memoria
                .Select(s => new {Sala = s,CantidadFunciones = listaAuxDeFunciones.Count(f => f.SalaId == s.Id) }).ToList(); // Ya en memoria

            ViewBag.SalasConCantidad = salasConCantidad;


            List<Pelicula> listaDePeliculas = context.Peliculas // Listo las peliculas
                .Include(p => p.Genero)  // Me agrega el Genero de la Pelicula en la lista creada
                .ToList();
            ViewBag.Peliculas = listaDePeliculas; // Mando la lista de peliculas a la Vista

            List<Sala> listaDeSalas = context.Salas.ToList(); // Listo las salas
            ViewBag.Salas = listaDeSalas; // Mando la lista de salas a la Vista

            List<Usuario> listaDeUsuarios = context.Usuarios // Listo los usuarios
                .Include(u => u.Rol)  // Me agrega el Rol al usuario en la lista creada
                .ToList(); 
            ViewBag.Usuarios = listaDeUsuarios; // Mando la lista de usuarios a la Vista

            return View();
        }
        [HttpPost] 
        public IActionResult EliminarFuncion(int id) // Eliminar Funcion via ID c/checkeo
        {
            var funcion = context.Funciones.Find(id);
            if (funcion != null)
            {
                context.Funciones.Remove(funcion);
                context.SaveChanges();
            }
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult EliminarPelicula(int id)
        {
            // Obtener todas las funciones asociadas a la película
            var funciones = context.Funciones.Where(f => f.PeliculaId == id).ToList();

            // Eliminar las funciones si existen
            if (funciones.Any())
            {
                context.Funciones.RemoveRange(funciones);
            }

            // Eliminar la película
            var pelicula = context.Peliculas.Find(id);
            context.Peliculas.Remove(pelicula);
            context.SaveChanges();

            TempData["Mensaje"] = "La película y sus funciones han sido eliminadas.";
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult EliminarSala(int id)
        {
            var tieneFunciones = context.Funciones.Where(f => f.SalaId == id).ToList();

            // Eliminar las funciones si existen
            if (tieneFunciones.Any())
            {
                context.Funciones.RemoveRange(tieneFunciones);
            }

            var sala = context.Salas.Find(id);
            if (sala != null)
            {
                context.Salas.Remove(sala);
                context.SaveChanges();
            }
            TempData["Mensaje"] = "La sala y sus funciones han sido eliminadas.";
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = context.Usuarios.Include(u => u.Tickets).Include(u => u.Rol).FirstOrDefault(u => u.Id == id);
            if (usuario == null) return RedirectToAction("Admin");

            context.Usuarios.Remove(usuario);
            context.SaveChanges();
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult ResetearPasswordUsuario(int id)
        {
            var usuario = context.Usuarios.FirstOrDefault(u => u.Id == id); // Busco el user por id
            if (usuario == null) return RedirectToAction("Admin"); // Si no lo encuentro vuelvo a la vista

            usuario.Contrasena = Convert.ToBase64String(RandomNumberGenerator.GetBytes(8)); // si existe, reseteo el pass
            context.Usuarios.Update(usuario); // actualizo el pass en el user
            context.SaveChanges(); // y guardo los cambios

            return RedirectToAction("Admin");
        }
    }
}
