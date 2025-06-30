using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PNT1_TP_Cine.Models;
using System.Security.Claims;

namespace PNT1_TP_Cine.Controllers
{
    public class AccountController : Controller
    {
        Context context = new Context();

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
            if (context.Usuarios.Any(u => u.Email == usuario.Email))
            {
                ModelState.AddModelError("Email", "Ya existe una cuenta registrada con este email.");
                return View(usuario);
            }

            // Asignar rol por defecto (cliente)
            usuario.RolId = 2;

            if (ModelState.IsValid)
            {
                context.Usuarios.Add(usuario);
                context.SaveChanges();
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
        [HttpGet("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string Email, string Contrasena, [FromForm] string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; 
            var usuario = context.Usuarios
                .FirstOrDefault(u => u.Email == Email && u.Contrasena == Contrasena);

            if (usuario == null)
            {
                ViewBag.Error = "Email o contraseña incorrectos.";
                return View();
            }

            // Crear los claims que representan al usuario
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.GivenName, usuario.Nombre),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.RolId.ToString())

            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


        // GET: /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Login");
        }

    }
}
