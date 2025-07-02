using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PNT1_TP_Cine.Models;
using Microsoft.AspNetCore.Authorization;

namespace PNT1_TP_Cine.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        Context context = new Context();

        public IActionResult Index()
        {
            List<Funcion> listaDeFunciones = context.Funciones.Include(f => f.Sala).ToList(); ; // Listo las funciones
            ViewBag.Funciones = listaDeFunciones; // Mando la lista de funciones a la Vista

            // Aca vamos a contar la cantidad de salas por cada funcion
            var listaAuxDeFunciones = context.Funciones.ToList(); // Trae funciones a memoria

            var salasConCantidad = context.Salas
                .AsEnumerable() // Trae salas a memoria para poder usar LINQ en memoria
                .Select(s => new { Sala = s, CantidadFunciones = listaAuxDeFunciones.Count(f => f.SalaId == s.Id) }).ToList(); // Ya en memoria

            ViewBag.SalasConCantidad = salasConCantidad;


            List<Pelicula> listaDePeliculas = context.Peliculas // Listo las peliculas
                .Include(p => p.Genero)  // Me agrega el Genero de la Pelicula en la lista creada
                .ToList();
            ViewBag.Peliculas = listaDePeliculas; // Mando la lista de peliculas a la Vista

            List<Sala> listaDeSalas = context.Salas.ToList(); // Listo las salas
            ViewBag.Salas = listaDeSalas; // Mando la lista de salas a la Vista

            List<Usuario> listaDeUsuarios = context.Usuarios // Listo los usuarios
                .Include(u => u.Rol)  // Me agrega el Rol al usuario en la lista creada
                .Where(u => u.Rol != null)
                .ToList();
            ViewBag.Usuarios = listaDeUsuarios; // Mando la lista de usuarios a la Vista
            ViewBag.Roles = context.Roles.ToList();

            return View();
        }

        [HttpGet]
        public IActionResult CrearSala() //Cuando el admin hace clic en "Crear Sala"
                                         //el controlador devuelve la vista con el formulario vacío para crear una nueva sala. 
        {
            return View(); //Devuelve la vista asociada a este método Views/Panel/CrearSala.cshtml
        }

        [HttpGet]
        public IActionResult CrearPelicula()
        {
            var generos = context.Generos.ToList();
            if (!generos.Any())
            {
                TempData["Error"] = "No hay géneros disponibles. Debes crear al menos un género primero.";
                return RedirectToAction("Index");
            }

            ViewBag.Generos = new SelectList(generos, "Id", "Nombre");
            return View();
        }

        [HttpGet]
        public IActionResult CrearFuncion()
        {
            // Obtengo la lista de películas desde la base de datos
            List<Pelicula> peliculas = context.Peliculas.ToList();

            // Transformo a SelectListItem para usar en el dropdown
            ViewBag.Peliculas = peliculas.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),     // El valor que se enviará en el formulario
                Text = p.Titulo              // El texto que se mostrará en el dropdown
            }).ToList();

            var salas = context.Salas.ToList();
            ViewBag.Salas = salas.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = "Sala " + s.Numero.ToString()
            }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult CrearSala(Sala sala)
        {
            if (ModelState.IsValid)
            {
                context.Salas.Add(sala);
                context.SaveChanges();
                TempData["Success"] = "Sala creada exitosamente.";
                return RedirectToAction("Index");
            }
            return View(sala);
        }

        [HttpPost]
        public IActionResult CrearPelicula(Pelicula pelicula)
        {

            if (ModelState.IsValid)
            {
                context.Peliculas.Add(pelicula);
                context.SaveChanges();
                TempData["Succes"] = "Película creada exitosamente.";
                return RedirectToAction("Index");
            }

            ViewBag.Generos = new SelectList(context.Generos.ToList(), "Id", "Nombre");
            TempData["Error"] = "Revisá los campos. Faltan datos obligatorios o hay errores.";
            return View(pelicula);
        }

        [HttpPost]
        public IActionResult CrearFuncion(Funcion funcion)
        {

            if (ModelState.IsValid)
            {
                context.Funciones.Add(funcion);
                context.SaveChanges();
                TempData["Success"] = "Función creada exitosamente.";
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, vuelvo a cargar los dropdowns para que el formulario no quede vacío
            List<Pelicula> peliculas = context.Peliculas.ToList();
            ViewBag.Peliculas = peliculas.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Titulo
            }).ToList();

            var salas = context.Salas.ToList();
            ViewBag.Salas = salas.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = "Sala " + s.Numero.ToString()
            }).ToList();

            return View(funcion);
        }

        [HttpGet] // Asegúrate de incluir este decorador
        public IActionResult CrearUsuario(bool fromAdmin = true) // Parámetro opcional
        {
            // Cargar datos necesarios (ej: roles para dropdown)
            ViewBag.Roles = context.Roles
                .Select(r => new SelectListItem(r.Nombre, r.Id.ToString()))
                .ToList();

            return View();
        }

        [HttpPost]
        public IActionResult CrearUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.RolId = usuario.RolId; // Mantiene el rol seleccionado
                context.Usuarios.Add(usuario);
                context.SaveChanges();
                TempData["Success"] = "Usuario creado exitosamente.";
                return RedirectToAction("Index");
            }
            else
            {
                // Recargar datos si hay errores
                ViewBag.Roles = context.Roles.ToList();
                TempData["Error"] = "El usuario no pudo ser creado.";
            }
                
            return View(usuario);
        }

        [HttpPost]
        public IActionResult EliminarFuncion(int id) // Eliminar Funcion via ID c/checkeo
        {
            var funcion = context.Funciones.Find(id);
            if (funcion != null)
            {
                TempData["Success"] = "Funcion eliminada.";
                context.Funciones.Remove(funcion);
                context.SaveChanges();
            } 
            else
            { 
                TempData["Error"] = "La funcion no existe.";
            }
            return RedirectToAction("Index");
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
            if (pelicula == null)
            {
                TempData["Error"] = "Error: La película que intentas eliminar no existe.";
                return RedirectToAction("Index");
            }
            context.Peliculas.Remove(pelicula);
            context.SaveChanges();

            TempData["Success"] = "La película y sus funciones han sido eliminadas.";
            return RedirectToAction("Index");
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
                TempData["Success"] = "La sala y sus funciones han sido eliminadas.";
            } else
            {
                TempData["Error"] = "La sala que intentas eliminar no existe.";
            }
                return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = context.Usuarios.Include(u => u.Tickets).Include(u => u.Rol).FirstOrDefault(u => u.Id == id);
            if (usuario == null) 
            {
                TempData["Error"] = "El usuario no existe.";
                return RedirectToAction("Index");
            }
            if (usuario.RolId == 1)
            {
                TempData["Error"] = "No puedes eliminar tu propio usuario administrador.";
                return RedirectToAction("Index");
            }
            TempData["Success"] = "Usuario eliminado exitosamente.";
            context.Usuarios.Remove(usuario);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ResetearPasswordUsuario(int id)
        {
            var usuario = context.Usuarios.FirstOrDefault(u => u.Id == id); // Busco el user por id
            if (usuario == null) return RedirectToAction("Index"); // Si no lo encuentro vuelvo a la vista

            usuario.Contrasena = Convert.ToBase64String(RandomNumberGenerator.GetBytes(8)); // si existe, reseteo el pass
            context.Usuarios.Update(usuario); // actualizo el pass en el user
            context.SaveChanges(); // y guardo los cambios

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ActualizarRolUsuario(int usuarioId, int rolId)
        {
            var usuario = context.Usuarios.Find(usuarioId);
            var rol = context.Roles.Find(rolId);

            if (usuario != null && rol != null)
            {
                usuario.RolId = rolId;
                context.SaveChanges();
                TempData["Success"] = "Rol de usuario actualizado exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo actualizar el rol del usuario.";
            }

            return RedirectToAction("Index");
        }
    }
}
