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

        [HttpPost]
        public IActionResult Elegir(int funcionId, int cantidad)
        {
            if (cantidad <= 0)
            {
                return BadRequest("La cantidad debe ser mayor a cero.");
            }

            var funcion = context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .FirstOrDefault(f => f.Id == funcionId);

            if (funcion == null)
            {
                return NotFound("La función seleccionada no existe.");
            }

            int ocupados = context.Tickets
                .Where(t => t.FuncionId == funcionId)
                .Sum(t => t.NumeroAsientos);

            int disponibles = funcion.Sala.Capacidad - ocupados;

            if (cantidad > disponibles)
            {
                TempData["Error"] = $"No hay suficientes asientos disponibles. Quedan {disponibles}.";
                return RedirectToAction("Detalle", "Peliculas", new { titulo = funcion.Pelicula.Titulo });
            }

            ViewBag.Funcion = funcion;
            ViewBag.Cantidad = cantidad;

            return View("Confirmar");
        }

        [HttpPost]
        public IActionResult ConfirmarCompra(int funcionId, int cantidad)
        {
            var funcion = context.Funciones
                .Include(f => f.Sala)
                .Include(f => f.Pelicula)
                .FirstOrDefault(f => f.Id == funcionId);

            if (funcion == null)
                return NotFound("La función no existe.");

            int ocupados = context.Tickets
                .Where(t => t.FuncionId == funcionId)
                .Sum(t => t.NumeroAsientos);

            int disponibles = funcion.Sala.Capacidad - ocupados;

            if (cantidad > disponibles)
            {
                TempData["Error"] = $"No hay suficientes asientos disponibles. Quedan {disponibles}.";
                return RedirectToAction("Detalle", "Peliculas", new { titulo = funcion.Pelicula.Titulo });
            }

            var email = User.Identity?.Name;
            var usuario = context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                return Unauthorized();

            var ticket = new Ticket
            {
                Numero = new Random().Next(100000, 999999),
                FechaCompra = DateTime.Now,
                Precio = cantidad * 1500,
                NumeroAsientos = cantidad,
                FuncionId = funcionId,
                UsuarioId = usuario.Id
            };

            context.Tickets.Add(ticket);
            context.SaveChanges();

            return RedirectToAction("Comprobante", new { id = ticket.Id });
        }

        public IActionResult Comprobante(int id)
        {
            var ticket = context.Tickets
                .Include(t => t.Funcion)
                    .ThenInclude(f => f.Pelicula)
                .Include(t => t.Funcion.Sala)
                .Include(t => t.Usuario)
                .FirstOrDefault(t => t.Id == id);

            if (ticket == null)
                return NotFound("El ticket no existe.");

            return View(ticket);
        }
    }
}
