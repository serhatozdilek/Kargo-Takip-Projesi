using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KargoTakibi.Models;

namespace KargoTakibi.Controllers
{
    [Authorize]
    public class KargoController : Controller
    {
        private readonly KargoTakipContext _context;

        public KargoController(KargoTakipContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kullaniciId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "KullaniciID").Value);
            var kargolar = await _context.Kargolar
                .Include(k => k.Hareketler)
                .Where(k => k.KullaniciID == kullaniciId)
                .OrderByDescending(k => k.KargoID)
                .ToListAsync();

            return View(kargolar);
        }

        public async Task<IActionResult> Detay(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kullaniciId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "KullaniciID").Value);
            var kargo = await _context.Kargolar
                .Include(k => k.Hareketler)
                .FirstOrDefaultAsync(k => k.KargoID == id && k.KullaniciID == kullaniciId);

            if (kargo == null)
            {
                return NotFound();
            }

            return View(kargo);
        }

        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Kargo kargo)
        {
            if (ModelState.IsValid)
            {
                kargo.KullaniciID = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "KullaniciID").Value);
                kargo.Durum = "Kabul Edildi";
                
                _context.Add(kargo);
                
                var hareket = new Hareket
                {
                    Kargo = kargo,
                    Tarih = DateTime.Now,
                    Durum = "Kabul Edildi"
                };
                _context.Hareketler.Add(hareket);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kargo);
        }

        public async Task<IActionResult> Duzenle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kullaniciId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "KullaniciID").Value);
            var kargo = await _context.Kargolar
                .FirstOrDefaultAsync(k => k.KargoID == id && k.KullaniciID == kullaniciId);

            if (kargo == null)
            {
                return NotFound();
            }

            return View(kargo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Duzenle(int id, Kargo kargo)
        {
            if (id != kargo.KargoID)
            {
                return NotFound();
            }

            var kullaniciId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "KullaniciID").Value);
            if (kargo.KullaniciID != kullaniciId)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kargo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KargoExists(kargo.KargoID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kargo);
        }

        private bool KargoExists(int id)
        {
            return _context.Kargolar.Any(e => e.KargoID == id);
        }
    }
}
