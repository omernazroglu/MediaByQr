namespace MediaByQr.Models
{
    public class Fotograf
    {

        public int FotografId { get; set; }
        public string DosyaAdı { get; set; }
        public string DosyaYolu { get; set; }
        public string DosyaTipi { get; set; }
        public DateTime YuklemeTarihi { get; set; }
        public int EtkinlikId { get; set; }
        public Etkinlik Etkinlik { get; set; } // ForeignKey
    }
}
