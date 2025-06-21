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
            List<Funcion> listaDeFunciones = context.Funciones.ToList(); // Listo las funciones
            ViewBag.Funciones = listaDeFunciones; // Mando la lista de funciones a la Vista

            List<Pelicula> listaDePeliculas = context.Peliculas.ToList(); // Listo las peliculas
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
        public IActionResult EliminarFuncion(int id)
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
            var tieneFunciones = context.Funciones.Any(f => f.PeliculaId == id);
            if (tieneFunciones)
            {
                TempData["Error"] = "No se puede eliminar la película porque tiene funciones asignadas.";
                return RedirectToAction("Admin");
            }

            var pelicula = context.Peliculas.Find(id);
            if (pelicula != null)
            {
                context.Peliculas.Remove(pelicula);
                context.SaveChanges();
            }
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult EliminarSala(int id)
        {
            var tieneFunciones = context.Funciones.Any(f => f.SalaId == id);
            if (tieneFunciones)
            {
                TempData["Error"] = "No se puede eliminar la sala porque tiene funciones asociadas.";
                return RedirectToAction("Admin");
            }

            var sala = context.Salas.Find(id);
            if (sala != null)
            {
                context.Salas.Remove(sala);
                context.SaveChanges();
            }
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = context.Usuarios.Include(u => u.Tickets).Include(u => u.Rol).FirstOrDefault(u => u.Id == id);
            if (usuario == null) return RedirectToAction("Admin");

            if (usuario.Rol.Nombre.Equals("Admin"))
            {
                TempData["Error"] = "No se puede eliminar un administrador";
                return RedirectToAction("Admin");
            }

            context.Usuarios.Remove(usuario);
            context.SaveChanges();
            return RedirectToAction("Admin");
        }
    }
}
