using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KargoTakibi.Models;

namespace KargoTakibi.Controllers
{
    [Authorize]
    public class HareketController : Controller
    {
        private readonly KargoTakipContext _context;

        public HareketController(KargoTakipContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> KargoHareketleri(int? id)
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

            return View(kargo.Hareketler.OrderByDescending(h => h.Tarih));
        }

        public async Task<IActionResult> YeniHareket(int? id)
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

            ViewBag.KargoID = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YeniHareket(int kargoId, string durum)
        {
            var kullaniciId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "KullaniciID").Value);
            var kargo = await _context.Kargolar
                .FirstOrDefaultAsync(k => k.KargoID == kargoId && k.KullaniciID == kullaniciId);

            if (kargo == null)
            {
                return NotFound();
            }

            var hareket = new Hareket
            {
                KargoID = kargoId,
                Durum = durum,
                Tarih = DateTime.Now
            };

            kargo.Durum = durum;

            _context.Hareketler.Add(hareket);
            await _context.SaveChangesAsync();

            return RedirectToAction("KargoHareketleri", new { id = kargoId });
        }

        public async Task<IActionResult> HareketListesi()
        {
            var kullaniciId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "KullaniciID").Value);
            var hareketler = await _context.Hareketler
                .Include(h => h.Kargo)
                .Where(h => h.Kargo.KullaniciID == kullaniciId)
                .OrderByDescending(h => h.Tarih)
                .ToListAsync();

            return View(hareketler);
        }
    }
}
