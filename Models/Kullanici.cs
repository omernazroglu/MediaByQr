using System.ComponentModel.DataAnnotations;

namespace MediaByQr.Models
{
    public class Kullanici
    {
        [Key]
        public int KullaniciId { get; set; }

        [Required]
        public string KullaniciAdi { get; set; }
        
        public string? Mail { get; set; }

         
        public string? Ad_Soyad { get; set; }

        [Required]
        public string SifreHash { get; set; }

        public ICollection<Etkinlik> Etkinlikler { get; set; }
    }
}
