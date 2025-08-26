using System.ComponentModel.DataAnnotations;

namespace MediaByQr.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gerekli.")]
        [Display(Name = "Kullanıcı Adı")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Şifre gerekli.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; }
    }
}
