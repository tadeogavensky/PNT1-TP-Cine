using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
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

                var passwordHasher = new PasswordHasher<Usuario>();
                usuario.Contrasena = passwordHasher.HashPassword(usuario, usuario.Contrasena);

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

            // Buscar el usuario por email
            var usuario = context.Usuarios.FirstOrDefault(u => u.Email == Email);

            if (usuario == null)
            {
                ViewBag.Error = "Email o contraseña incorrectos.";
                return View();
            }

            // Verificar el hash de la contraseña
            var passwordHasher = new PasswordHasher<Usuario>();
            var result = passwordHasher.VerifyHashedPassword(usuario, usuario.Contrasena, Contrasena);

            if (result != PasswordVerificationResult.Success)
            {
                ViewBag.Error = "Email o contraseña incorrectos.";
                return View();
            }

            // Asignar rol
            string rolNombre = usuario.RolId switch
            {
                1 => "Admin",
                2 => "Cliente",
                _ => "Guest"
            };

            // Crear los claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.Email),
        new Claim(ClaimTypes.GivenName, usuario.Nombre),
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(ClaimTypes.Role, rolNombre)
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Iniciar sesión
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Redirección
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Logout
        [HttpGet]
        public IActionResult Logout(string returnUrl = null)
        {
             HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Si el returnUrl es del Admin, redirige al Home
            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith("/Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Login");
        }

    }
}
