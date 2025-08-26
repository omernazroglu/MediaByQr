using MediaByQr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static QRCoder.PayloadGenerator;
using System.Net.Mail;
using System.Net;

namespace MediaByQr.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController (ApplicationDbContext context)
        {
               _context = context;
        }
        public IActionResult Login()
        {

            return View();
        }   
        [HttpPost]
        public async Task<IActionResult> Login(string KullaniciAdi, string SifreHash)
        {

            
                // Kullanıcı adı ile kullanıcıyı bul
                var user = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.KullaniciAdi == KullaniciAdi);
                if (user == null)
                {
                    TempData["LoginError"] = "Kullanıcı adı veya şifre hatalı!";
                    return RedirectToAction("Login");
                }

                // Şifreyi doğrula
                bool isValid = BCrypt.Net.BCrypt.Verify(SifreHash, user.SifreHash);
                if (!isValid)
                {
                TempData["LoginError"] = "Kullanıcı adı veya şifre hatalı!";
                    return RedirectToAction("Login");
                }

            // Giriş başarılı → session, cookie vs. ayarlayabilirsin
            ViewBag.KullaniciAdi = KullaniciAdi;
            TempData["KullaniciAdi"] = user.KullaniciAdi;

            TempData["LoginSuccess"] = "Başarıyla giriş yaptınız!";
            return RedirectToAction("Login");


        }
       
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Kullanici kullanici)
        {
            
            if (_context.Kullanicilar.Any(o => o.KullaniciAdi == kullanici.KullaniciAdi))
            {
                TempData["RegisterFalse"] = "Birden fazla ayni kullanici";
                return RedirectToAction("Create");

            }
            kullanici.SifreHash = BCrypt.Net.BCrypt.HashPassword(kullanici.SifreHash);
            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();
            TempData["RegisterSuccess"] = "Kayıt başarılı!";
            return RedirectToAction("Create");


        }

        public async Task<IActionResult> Profile(string kullaniciAdi)
        {
            var user= await _context.Kullanicilar.FirstOrDefaultAsync(k=>k.KullaniciAdi==kullaniciAdi);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult ChangePassword(string OldPassword, string NewPassword, string ConfirmNewPassword,string kullaniciAdi)
        {
            var user = _context.Kullanicilar.FirstOrDefault(k=>k.KullaniciAdi==kullaniciAdi);

            if (user == null)
                return NotFound();

            // Eski şifre doğrulama
            bool isOldPasswordCorrect = BCrypt.Net.BCrypt.Verify(OldPassword, user.SifreHash);

            if (!isOldPasswordCorrect)
            {
                // Eski şifre yanlış
                TempData["Error"] = "Eski şifre yanlış!";
                return RedirectToAction("Profile", new { kullaniciAdi = user.KullaniciAdi });
            }

            // Yeni şifreyi hashle
            string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            user.SifreHash = newHashedPassword;
            _context.SaveChanges();

            TempData["Success"] = "Şifre başarıyla değiştirildi!";
            return RedirectToAction("Profile", new {kullaniciAdi=user.KullaniciAdi});
        }

        public async Task<IActionResult> ChangeInformation(string Ad_Soyad,string KullaniciAdi,int kullaniciId)
        {
            var user=await _context.Kullanicilar.FirstOrDefaultAsync(k=>k.KullaniciId==kullaniciId);
           if(user == null) return NotFound();

            if (_context.Kullanicilar.Any(o => o.KullaniciAdi == KullaniciAdi))
            {
                TempData["InformationFalse"] = "Birden fazla ayni kullanici";
                return RedirectToAction("Profile", new { kullaniciAdi = user.KullaniciAdi });

            }
            user.KullaniciAdi=KullaniciAdi;
            user.Ad_Soyad=Ad_Soyad;
           _context.Kullanicilar.Update(user);
            await _context.SaveChangesAsync();
            TempData["InformationSuccess"] = "Bilgi Güncelleme başarılı!";
            return RedirectToAction("Profile", new { kullaniciAdi =user.KullaniciAdi});
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string kullaniciAdi,string Email)
        {
            var user=_context.Kullanicilar.FirstOrDefault(k=>k.KullaniciAdi==kullaniciAdi);
            if (user == null)
            {
                TempData["UserFalse"] = "user bulunamadi";
                return View();
            }
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
          + "|" + DateTime.UtcNow.AddMinutes(2).ToString("O")+"|"+"&username="+kullaniciAdi;
            DateTime tokenDateTime= DateTime.Now;
            string resetLink = $"https://localhost:7122/Account/RefreshPassword?token={token}";
          
            string konu = "Şifre Sıfırlama";
            string icerik = $"Şifrenizi sıfırlamak için bu bağlantıya tıklayın: {resetLink}";

            try
            {

                string alıcıEmail = Email;
                string senderEmail = "emine1naziroglu@gmail.com"; // kendi Gmail adresin
                string appPassword = "bdvl hdwf wehe fmbi"; // uygulama şifresi

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(alıcıEmail); // sana gelen mail

                // Kullanıcının maili ReplyTo olarak eklenir


                mail.Subject = konu;
                mail.IsBodyHtml = true;
                mail.Body = icerik;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(senderEmail, appPassword);

                smtp.EnableSsl = true;
                smtp.Send(mail);
                TempData["MailSuccess"] = "mailgonderildi";
                return View();

            }
            catch (Exception)
            {
                // Hata mesajı
             //  return View();
              
            }


            return View();
        }

            public IActionResult RefreshPassword(string token,string username)
            {
                ViewBag.username=username;
                ViewBag.Token=token;
                string usernameparam=username;
                string tokenparam = token;
                if (token == null)
                {
                    return NotFound();
                }
                if (token != null)
                {
                    var parts = token.Split('|');
                    var expireTime = DateTime.Parse(parts[1]);

                    if (DateTimeOffset.UtcNow > expireTime)
                    {

                    ViewBag.TokenExpired = true; // SweetAlert için flag
                    return View();
                }
              
                    return View();

                }
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> RefreshPassword(string NewPassword,string username,string token)
            {
                string tokenparam=token;
                var user= await _context.Kullanicilar.FirstOrDefaultAsync(k=>k.KullaniciAdi==username);
                if (user == null) {
                    return NotFound();
                }
                user.SifreHash = BCrypt.Net.BCrypt.HashPassword(NewPassword);
                _context.Kullanicilar.Update(user);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Şifre yenileme başarılı!";
                return RedirectToAction("RefreshPassword", new {token=tokenparam,username = user.KullaniciAdi});

            
            }


    }
}
