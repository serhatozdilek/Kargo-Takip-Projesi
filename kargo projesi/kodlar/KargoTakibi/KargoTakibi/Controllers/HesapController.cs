using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using KargoTakibi.Models;

namespace KargoTakibi.Controllers
{
    public class HesapController : Controller
    {
        private readonly KargoTakipContext _context;

        public HesapController(KargoTakipContext context)
        {
            _context = context;
        }

        public IActionResult Giris()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Giris(string email, string sifre)
        {
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.Email == email && k.Sifre == sifre);

            if (kullanici == null)
            {
                ModelState.AddModelError("", "Geçersiz email veya şifre");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, kullanici.AdSoyad),
                new Claim(ClaimTypes.Email, kullanici.Email),
                new Claim("KullaniciID", kullanici.KullaniciID.ToString())
            };

            var kimlik = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(kimlik);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Kayit()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Kayit(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                var emailKontrol = await _context.Kullanicilar
                    .AnyAsync(k => k.Email == kullanici.Email);

                if (emailKontrol)
                {
                    ModelState.AddModelError("Email", "Bu email adresi zaten kullanılıyor");
                    return View(kullanici);
                }

                _context.Add(kullanici);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Giris));
            }
            return View(kullanici);
        }

        public async Task<IActionResult> Cikis()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
