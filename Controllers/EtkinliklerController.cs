using iTextSharp.text;
using iTextSharp.text.pdf.qrcode;
using MediaByQr.Migrations;
using MediaByQr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Drawing;
using static QRCoder.PayloadGenerator;

namespace MediaByQr.Controllers
{
    public class EtkinliklerController : Controller
    {

        private readonly ApplicationDbContext _context;

        public EtkinliklerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string kullaniciAdi)
            {

            ViewBag.KullaniciAdi = kullaniciAdi;
            var kullanici = _context.Kullanicilar
                .Include(k => k.Etkinlikler)
                .ThenInclude(e=>e.Fotograflar)// navigation property
                .FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi);

            var model = kullanici?.Etkinlikler
                .Select(e => new EtkinlikViewModel
                {
                    Id = e.EtkinlikId,
                    Ad = e.EtkinlikAdi,
                    FotoUrls= e.Fotograflar.Select(f=>f.DosyaYolu).ToList
                    (),
                    QrUrl=e.QrCodeUrl,
                    OlusturulmaTarihi=e.OlusturulmaTarihi
                    
                })
                .ToList() ?? new List<EtkinlikViewModel>();

            return View(model);
        }

        public IActionResult Create(string kullaniciAdi)
        {
            ViewBag.KullaniciAdi = kullaniciAdi;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create(Etkinlik etkinlik, string KullaniciAdi)
        {
            if (etkinlik != null)
            {
                // Giriş yapan kullanıcıyı bul
                var user = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == KullaniciAdi);
                string qrText = $"{user.KullaniciId}_{DateTime.UtcNow.Ticks}";

                // Tarayıcıda açılabilir URL olarak QR kodun içine göm
                string qrUrl = Url.Action("EtkinlikUpload","Etkinlikler",new { qr= qrText },Request.Scheme);


                using (var qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrUrl, QRCodeGenerator.ECCLevel.Q);
                    var qrCode = new PngByteQRCode(qrCodeData);
                    byte[] qrCodeAsPng = qrCode.GetGraphic(20);
                    var QrImage = Convert.ToBase64String(qrCodeAsPng);
                    etkinlik.QrCodeUrl = QrImage;
                    etkinlik.QrCodeKey = qrText;
                    etkinlik.KullaniciId = user.KullaniciId; // ilişkiyi kur
                    _context.Etkinlikler.Add(etkinlik);
                    _context.SaveChanges();
                    ViewBag.KullaniciAdi=user.KullaniciAdi; 
                    return RedirectToAction("Index", "Etkinlikler", new {kullaniciAdi=KullaniciAdi});
                }
            }

            return View();
        }
 public IActionResult EtkinlikUpload(string qr)
{
    if (string.IsNullOrEmpty(qr))
        return NotFound();






  



           
                var etkinlik = _context.Etkinlikler.FirstOrDefault(e =>e.QrCodeKey==qr);
                if (etkinlik == null)
                    return NotFound();
            var user = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciId == etkinlik.KullaniciId);
            ViewBag.userNameSurname = user.Ad_Soyad;
                return View(etkinlik);
           







               


    // long ticks = long.Parse(parts[1]); // istersen kullanabilirsin

    // Kullanıcıya veya etkinliğe ait modeli al
    
   

    // Foto/Video yükleme formu
}








        [HttpPost]
        public async Task<IActionResult> UploadFiles(int etkinlikId, List<IFormFile> media)
        {
            var etkinlik = await _context.Etkinlikler
                                         .Include(e => e.Fotograflar)
                                         .FirstOrDefaultAsync(e => e.EtkinlikId == etkinlikId);

            if (etkinlik == null) return NotFound();

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            foreach (var file in media)
            {
                if (file.Length > 0)
                {
                    // Benzersiz dosya adı oluştur
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Dosya tipini belirle (fotoğraf mı video mu)
                    string fileType = file.ContentType.StartsWith("image") ? "image" : "video";

                    // Media nesnesi oluştur
                    var medianew = new Fotograf
                    {
                        DosyaAdı = uniqueFileName,
                        DosyaYolu = "/uploads/" + uniqueFileName,
                        DosyaTipi = fileType,
                        EtkinlikId = etkinlik.EtkinlikId
                    };

                    etkinlik.Fotograflar.Add(medianew);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = etkinlikId }); // örnek: detay sayfasına dön
        }
        public async Task<IActionResult> Details(int id)
        {
            var etkinlik =  await _context.Etkinlikler
                                            .Include(e => e.Fotograflar)
                                            .FirstOrDefaultAsync(e => e.EtkinlikId == id);
            if (etkinlik == null)
                return NotFound();
            var user= await _context.Kullanicilar.FirstOrDefaultAsync(k=>k.KullaniciId==etkinlik.KullaniciId);
            ViewBag.KullaniciAdi = user.KullaniciAdi;
            return View(etkinlik);
        }
        public async Task< IActionResult> Delete(int id)
        {
            var etkinlik = await _context.Etkinlikler.FirstOrDefaultAsync(e => e.EtkinlikId == id);
            if (etkinlik == null)
            {
                return NotFound();
            }
            
            var user = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.KullaniciId == etkinlik.KullaniciId);
            if (user == null)
            {
                return NotFound();
            }

            _context.Etkinlikler.Remove(etkinlik);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { kullaniciAdi = user.KullaniciAdi });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id,string username)
        {
            ViewBag.KullaniciAdi=username;
            var etkinlik = await _context.Etkinlikler.FirstOrDefaultAsync(e => e.EtkinlikId == id);
            if (etkinlik == null)
            {
                return NotFound();
            }

            return View(etkinlik);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Edit(string EtkinlikAdi,DateTime OlusturulmaTarihi,int id)
        {
            var etkinlik = await _context.Etkinlikler.FirstOrDefaultAsync(e => e.EtkinlikId == id);
            if (etkinlik == null)
            {
                // Giriş yapan kullanıcıyı bul
                return NotFound();

            }
            var user = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.KullaniciId == etkinlik.KullaniciId);
            if (user == null)
            {
                return NotFound();
            }
            etkinlik.OlusturulmaTarihi = OlusturulmaTarihi;
            etkinlik.EtkinlikAdi = EtkinlikAdi;
            _context.Etkinlikler.Update(etkinlik);
                    _context.SaveChanges();
                    
                    return RedirectToAction("Index", "Etkinlikler", new { kullaniciAdi = user.KullaniciAdi });
                }
            

            
        

    }


}
