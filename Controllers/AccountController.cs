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
            // Validar que el email no esté ya registrado
            if (_context.Usuarios.Any(u => u.Email == usuario.Email))
            {
                ModelState.AddModelError("Email", "Ya existe una cuenta registrada con este email.");
                return View(usuario);
            }

            // Asignar rol por defecto (cliente)
            usuario.RolId = 2;

                if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                TempData["RegistroExitoso"] = "¡Cuenta creada exitosamente! Ahora podés iniciar sesión.";
                return RedirectToAction("Login");
            }
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"❌ {key}: {error.ErrorMessage}");
                }
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
                HttpContext.Session.SetString("UsuarioLogueado", usuario.Email);
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                HttpContext.Session.SetInt32("UsuarioRol", usuario.RolId); 

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email o contraseña incorrectos.";
            return View();
        }


        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); //Borra toda la sesión
            return RedirectToAction("Login", "Account");
        }
    }
}
