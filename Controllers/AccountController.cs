using Microsoft.AspNetCore.Mvc;
using PNT1_TP_Cine.Models;
using System.Linq;

namespace PNT1_TP_Cine.Controllers
{
    public class AccountController : Controller
    {
        private readonly Context _context;

        public AccountController(Context context)
        {
            _context = context;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Guarda el usuario en la base de datos
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                // Después de registrarse, redirige al Login
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string Email, string Contrasena)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == Email && u.Contrasena == Contrasena);

            if (usuario != null)
            {
                // Se guarda el Email en TempData como ejemplo de sesión
                TempData["UsuarioLogueado"] = usuario.Email;

                // Podés redirigir al home o al panel
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email o contraseña incorrectos";
            return View();
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Login");
        }
    }
}
