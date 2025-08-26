namespace MediaByQr.Models
{
    public class EtkinlikViewModel

    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public List<string> FotoUrls { get; set; } // Yüklü resmin herkese açık yolu

        public string QrUrl { get; set; }

        public DateTime OlusturulmaTarihi { get; set; }
    }
}
