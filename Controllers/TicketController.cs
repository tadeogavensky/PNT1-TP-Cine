using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNT1_TP_Cine.Models;



namespace PNT1_TP_Cine.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        Context context = new Context();

        // Metodo para obtener una función por id
        private Funcion? ObtenerFuncionConDatos(int funcionId)
        {
            return context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .FirstOrDefault(f => f.Id == funcionId);
        }

        // Obtener la cantidad de asientos disponibles para la función
        private int ObtenerAsientosDisponibles(int funcionId, int capacidadSala)
        {
            int ocupados = context.Tickets
                .Where(t => t.FuncionId == funcionId)
                .Sum(t => t.NumeroAsientos);

            return capacidadSala - ocupados;
        }

        [HttpPost]
        public IActionResult Elegir(int funcionId, int cantidad)
        {
            if (cantidad <= 0)
                return BadRequest("La cantidad debe ser mayor a cero.");

            // Obtengo la función 
            var funcion = ObtenerFuncionConDatos(funcionId);
            if (funcion == null)
                return NotFound("La función seleccionada no existe.");

            // Obtengo la cantidad de asientos disponibles
            int disponibles = ObtenerAsientosDisponibles(funcionId, funcion.Sala.Capacidad);

            // Si la cantidad es mayor a los disponibles, tiro error
            if (cantidad > disponibles)
            {
                return RedirectToAction("Detalle", "Peliculas", new { titulo = funcion.Pelicula.Titulo });
            }

            ViewBag.Funcion = funcion;
            ViewBag.Cantidad = cantidad;
            return View("Confirmar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarCompra(int funcionId, int cantidad)
        {
            // Obtengo la función con los datos necesarios
            var funcion = ObtenerFuncionConDatos(funcionId);
            if (funcion == null)
                return NotFound("La función no existe.");

            int disponibles = ObtenerAsientosDisponibles(funcionId, funcion.Sala.Capacidad);
            if (cantidad > disponibles)
            {
                return RedirectToAction("Detalle", "Peliculas", new { titulo = funcion.Pelicula.Titulo });
            }




            // Verifico que el usuario este logueado
            var email = User.Identity?.Name;
            Console.WriteLine($"EMAIL (Identity.Name): {email}");

            var usuario = context.Usuarios.FirstOrDefault(u => u.Email == email);
            if (usuario == null)
                return Unauthorized();

            // Creo el ticket para el usuario
            var ticket = new Ticket
            {
                Numero = new Random().Next(100000, 999999),
                FechaCompra = DateTime.Now,
                Precio = cantidad * 1500,
                NumeroAsientos = cantidad,
                FuncionId = funcionId,
                UsuarioId = usuario.Id
            };

            // Guardo el ticket en la base de datos
            context.Tickets.Add(ticket);
            context.SaveChanges();

            // Redirijo al usuario a la vista del comprobante
            return RedirectToAction("CompraExitosa", new { id = ticket.Id });

        }

        public IActionResult CompraExitosa(int id)
        {
            if (id == 0)
                return RedirectToAction("Index", "Home");

            ViewBag.TicketId = id;
            return View();
        }



        public IActionResult Comprobante(int id)
        {
            // Verifico que el ticket exista
            var ticket = context.Tickets
                .Include(t => t.Funcion)
                    .ThenInclude(f => f.Pelicula)
                .Include(t => t.Funcion.Sala)
                .Include(t => t.Usuario)
                .FirstOrDefault(t => t.Id == id);

            if (ticket == null)
                return NotFound("El ticket no existe.");

            // Devuelvo la vista del comprobante con los datos del ticket

            return View(ticket);
        }
    }
}
