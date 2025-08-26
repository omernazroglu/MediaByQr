using System.ComponentModel.DataAnnotations;

namespace MediaByQr.Models
{
    public class Etkinlik
    {

        // Birincil anahtar (Primary Key). EF Core otomatik olarak kimlik (Identity) olarak ayarlar.
        public int EtkinlikId { get; set; }

        // Etkinlik adının girilmesi zorunlu.
        [Required(ErrorMessage = "Etkinlik Adı boş bırakılamaz.")]
        [Display(Name = "Etkinlik Adı")]
        public string EtkinlikAdi { get; set; }

        // Her etkinliğin kendine özel QR kodunun yönleneceği URL.
        // İlk oluşturulduğunda boş olabilir, sonradan doldurulacaktır.
        public string QrCodeUrl { get; set; }
        public string QrCodeKey { get; set; }

        // Etkinliğin oluşturulma tarihi.
        [Required(ErrorMessage = "Etkinlik Tarihi Boş Bırakılamaz")]
        [Display(Name = "Etkinlik Tarihi")]
        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;

        // Yabancı anahtar (Foreign Key). Etkinlik sahibinin KullaniciId'sini tutar.
        // [Required] ile işaretlememize gerek yok çünkü controller'da biz atayacağız.
        public int KullaniciId { get; set; }

        // Navigasyon özelliği. İlişkili olduğu Kullanici nesnesini temsil eder.
        // Formdan bu alanın gelmesi beklenmediği için [Required] kullanmıyoruz.
        public Kullanici Kullanici { get; set; }

        // Navigasyon özelliği. Bu etkinliğe ait fotoğrafların listesini tutar.
        // Bu liste veritabanında direkt bir sütun olarak saklanmaz.
        public List<Fotograf> Fotograflar { get; set; }= new List<Fotograf>();

    }
}
