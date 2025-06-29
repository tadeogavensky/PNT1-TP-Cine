using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PNT1_TP_Cine.Models;

namespace PNT1_TP_Cine.Controllers
{
    public class AdminController : Controller
    {
        //Context context = new Context();
        private readonly Context _context;

        public AdminController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Funcion> listaDeFunciones = _context.Funciones.Include(f => f.Sala).ToList(); ; // Listo las funciones
            ViewBag.Funciones = listaDeFunciones; // Mando la lista de funciones a la Vista

            // Aca vamos a contar la cantidad de salas por cada funcion
            var listaAuxDeFunciones = _context.Funciones.ToList(); // Trae funciones a memoria

            var salasConCantidad = _context.Salas
                .AsEnumerable() // Trae salas a memoria para poder usar LINQ en memoria
                .Select(s => new { Sala = s, CantidadFunciones = listaAuxDeFunciones.Count(f => f.SalaId == s.Id) }).ToList(); // Ya en memoria

            ViewBag.SalasConCantidad = salasConCantidad;


            List<Pelicula> listaDePeliculas = _context.Peliculas // Listo las peliculas
                .Include(p => p.Genero)  // Me agrega el Genero de la Pelicula en la lista creada
                .ToList();
            ViewBag.Peliculas = listaDePeliculas; // Mando la lista de peliculas a la Vista

            List<Sala> listaDeSalas = _context.Salas.ToList(); // Listo las salas
            ViewBag.Salas = listaDeSalas; // Mando la lista de salas a la Vista

            List<Usuario> listaDeUsuarios = _context.Usuarios // Listo los usuarios
                .Include(u => u.Rol)  // Me agrega el Rol al usuario en la lista creada
                .ToList();
            ViewBag.Usuarios = listaDeUsuarios; // Mando la lista de usuarios a la Vista

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
            var generos = _context.Generos.ToList();
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
            List<Pelicula> peliculas = _context.Peliculas.ToList();

            // Transformo a SelectListItem para usar en el dropdown
            ViewBag.Peliculas = peliculas.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),     // El valor que se enviará en el formulario
                Text = p.Titulo              // El texto que se mostrará en el dropdown
            }).ToList();

            var salas = _context.Salas.ToList();
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
                _context.Salas.Add(sala);
                _context.SaveChanges();
                TempData["Success"] = "Sala creada exitosamente";
                return RedirectToAction("Index");
            }
            return View(sala);
        }

        [HttpPost]
        public IActionResult CrearPelicula(Pelicula pelicula)
        {

            if (ModelState.IsValid)
            {
                _context.Peliculas.Add(pelicula);
                _context.SaveChanges();
                TempData["Success"] = "Película creada exitosamente";
                return RedirectToAction("Index");
            }

            ViewBag.Generos = new SelectList(_context.Generos.ToList(), "Id", "Nombre");
            TempData["Error"] = "Revisá los campos. Faltan datos obligatorios o hay errores.";
            return View(pelicula);
        }

        [HttpPost]
        public IActionResult CrearFuncion(Funcion funcion)
        {

            if (ModelState.IsValid)
            {
                _context.Funciones.Add(funcion);
                _context.SaveChanges();
                TempData["Success"] = "Función creada exitosamente";
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, vuelvo a cargar los dropdowns para que el formulario no quede vacío
            List<Pelicula> peliculas = _context.Peliculas.ToList();
            ViewBag.Peliculas = peliculas.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Titulo
            }).ToList();

            var salas = _context.Salas.ToList();
            ViewBag.Salas = salas.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = "Sala " + s.Numero.ToString()
            }).ToList();

            return View(funcion);
        }


        [HttpPost]
        public IActionResult EliminarFuncion(int id) // Eliminar Funcion via ID c/checkeo
        {
            var funcion = _context.Funciones.Find(id);
            if (funcion != null)
            {
                _context.Funciones.Remove(funcion);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarPelicula(int id)
        {
            // Obtener todas las funciones asociadas a la película
            var funciones = _context.Funciones.Where(f => f.PeliculaId == id).ToList();

            // Eliminar las funciones si existen
            if (funciones.Any())
            {
                _context.Funciones.RemoveRange(funciones);
            }

            // Eliminar la película
            var pelicula = _context.Peliculas.Find(id);
            _context.Peliculas.Remove(pelicula);
            _context.SaveChanges();

            TempData["Mensaje"] = "La película y sus funciones han sido eliminadas.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarSala(int id)
        {
            var tieneFunciones = _context.Funciones.Where(f => f.SalaId == id).ToList();

            // Eliminar las funciones si existen
            if (tieneFunciones.Any())
            {
                _context.Funciones.RemoveRange(tieneFunciones);
            }

            var sala = _context.Salas.Find(id);
            if (sala != null)
            {
                _context.Salas.Remove(sala);
                _context.SaveChanges();
            }
            TempData["Mensaje"] = "La sala y sus funciones han sido eliminadas.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = _context.Usuarios.Include(u => u.Tickets).Include(u => u.Rol).FirstOrDefault(u => u.Id == id);
            if (usuario == null) return RedirectToAction("Index");

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ResetearPasswordUsuario(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id); // Busco el user por id
            if (usuario == null) return RedirectToAction("Index"); // Si no lo encuentro vuelvo a la vista

            usuario.Contrasena = Convert.ToBase64String(RandomNumberGenerator.GetBytes(8)); // si existe, reseteo el pass
            _context.Usuarios.Update(usuario); // actualizo el pass en el user
            _context.SaveChanges(); // y guardo los cambios

            return RedirectToAction("Index");
        }
    }
}
