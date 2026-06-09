<div align="center">

<img src="https://readme-typing-svg.demolab.com?font=Inter&weight=700&size=36&pause=1000&color=7C3AED&center=true&vCenter=true&width=650&lines=📸+MediaByQR;QR+Kod+ile+Medya+Paylaşım+Platformu" alt="MediaByQR" />

<br/>

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/Entity_Framework_Core-SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)
![QR Code](https://img.shields.io/badge/QRCoder-Entegre-7C3AED?style=for-the-badge&logo=qrcode&logoColor=white)
![BCrypt](https://img.shields.io/badge/BCrypt-Şifre_Güvenliği-DC2626?style=for-the-badge&logo=letsencrypt&logoColor=white)

<br/>

<p align="center">
  <b>Etkinlik oluşturun → QR kodu yazdırın → Katılımcılar telefonlarıyla tarayıp anında fotoğraf ve video yüklesin.</b><br/>
  Düğünler, mezuniyetler, şirket etkinlikleri ve daha fazlası için medya toplama platformu.
</p>

</div>

---

## 🎯 Proje Fikri

Bir etkinlikte onlarca kişinin çektiği fotoğrafları tek bir yerde toplamak zordur. **MediaByQR**, bu sorunu çözmek için tasarlanmıştır:

1. 🎉 **Organizatör** sisteme giriş yapar ve bir etkinlik oluşturur
2. 📲 **Sistem otomatik olarak** o etkinliğe özel bir QR kod üretir
3. 🖨️ **QR kodu** etkinlik alanına asılır / davetiyeye eklenir
4. 👥 **Misafirler** telefon kameralarıyla QR'yi tarar → anında yükleme sayfasına ulaşır
5. 🖼️ **Fotoğraf & videolar** gerçek zamanlı olarak sistemde toplanır

---

## ✨ Özellikler

<table>
<tr>
<td width="50%">

### 🔐 Kullanıcı Sistemi
- Kayıt & Giriş (BCrypt ile şifrelenmiş)
- Profil düzenleme (ad-soyad, kullanıcı adı)
- Şifre değiştirme (eski şifre doğrulamalı)
- **Şifremi unuttum** → e-posta ile sıfırlama linki
- Token tabanlı, süreli sıfırlama (2 dk)

</td>
<td width="50%">

### 📅 Etkinlik Yönetimi
- Etkinlik oluştur, düzenle, sil
- Her etkinliğe **özel QR kod** otomatik üretilir
- QR kod → yükleme sayfasına doğrudan yönlendirir
- Etkinlik tarihi ve adı düzenlenebilir
- Etkinliğe ait tüm medyaları görüntüle

</td>
</tr>
<tr>
<td width="50%">

### 📸 Medya Yükleme
- QR tarama ile giriş gerektirmeden yükleme
- **Fotoğraf** (jpg, jpeg, png) ve **Video** (mp4) desteği
- Çoklu dosya yükleme (birden fazla aynı anda)
- UUID ile benzersiz dosya adı (çakışma önlenir)
- Dosya tipi otomatik algılama (image / video)

</td>
<td width="50%">

### 🛡️ Güvenlik
- BCrypt ile şifre hash'leme
- Anti-Forgery Token (CSRF koruması)
- Zaman sınırlı şifre sıfırlama token'ı
- Benzersiz QR anahtar sistemi (tahmin edilemez)

</td>
</tr>
</table>

---

## 🔄 Kullanım Akışı

```
[Organizatör Girişi]
       │
       ▼
[Yeni Etkinlik Oluştur]  ──→  [QR Kod Otomatik Üretilir]
                                        │
                              [QR'yi Yazdır / Paylaş]
                                        │
                              [Misafir QR'yi Tarar]
                                        │
                              [Yükleme Sayfası Açılır]
                                        │
                          [Fotoğraf / Video Seç & Yükle]
                                        │
                              [Medya Etkinliğe Eklenir]
                                        │
                         [Organizatör Panelinde Görünür] ✅
```

---

## 🛠️ Teknoloji Yığını

| Katman | Teknoloji | Açıklama |
|--------|-----------|----------|
| **Backend** | ASP.NET Core 8 MVC | Web framework |
| **ORM** | Entity Framework Core | Code-First, SQLite |
| **Veritabanı** | SQLite (`QrPhotoDb.db`) | Taşınabilir, sunucusuz |
| **QR Üretimi** | QRCoder | PNG formatında QR kodu |
| **PDF** | iTextSharp | PDF/QR entegrasyonu |
| **Güvenlik** | BCrypt.Net | Şifre hash'leme |
| **E-posta** | System.Net.Mail (SMTP) | Şifre sıfırlama maili |
| **Frontend** | HTML5, CSS3, JavaScript | Bootstrap ile |

---

## 🗃️ Veritabanı Modeli

```
┌─────────────────┐         ┌──────────────────────┐         ┌─────────────────┐
│   Kullanici     │         │      Etkinlik         │         │    Fotograf     │
├─────────────────┤         ├──────────────────────┤         ├─────────────────┤
│ KullaniciId  PK │◄──┐     │ EtkinlikId       PK  │◄──┐     │ FotografId   PK │
│ KullaniciAdi    │   └─────│ KullaniciId      FK  │   └─────│ EtkinlikId   FK │
│ Ad_Soyad        │         │ EtkinlikAdi          │         │ DosyaAdı        │
│ Mail            │         │ QrCodeUrl (base64)   │         │ DosyaYolu       │
│ SifreHash       │         │ QrCodeKey (unique)   │         │ DosyaTipi       │
└─────────────────┘         │ OlusturulmaTarihi    │         │ YuklemeTarihi   │
                            └──────────────────────┘         └─────────────────┘
```

---

## 🚀 Kurulum & Çalıştırma

### Ön Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/)

> SQLite kullandığı için ayrıca bir veritabanı sunucusu kurmanıza gerek **yoktur**.

### Adım Adım

```bash
# 1. Repoyu klonlayın
git clone https://github.com/omernazroglu/MediaByQr.git
cd MediaByQr

# 2. Bağımlılıkları yükleyin
dotnet restore

# 3. Veritabanını oluşturun
dotnet ef database update

# 4. Uygulamayı başlatın
dotnet run
```

Tarayıcınızda açın → **https://localhost:7122**

---

## ⚙️ E-posta Ayarları (Şifre Sıfırlama için)

`AccountController.cs` içinde SMTP ayarlarını doldurun:

```csharp
string senderEmail = "your-email@gmail.com";   // Gmail adresiniz
string appPassword  = "xxxx-xxxx-xxxx-xxxx";   // Gmail Uygulama Şifresi
```

> **Not:** Gmail'de "Uygulama Şifresi" oluşturmak için Google Hesabı → Güvenlik → 2 Adımlı Doğrulama → Uygulama şifreleri

---

## 📁 Proje Yapısı

```
MediaByQr/
├── Controllers/
│   ├── AccountController.cs     # Kayıt, giriş, profil, şifre sıfırlama
│   ├── EtkinliklerController.cs # Etkinlik CRUD + QR üretimi + medya yükleme
│   └── HomeController.cs        # Ana sayfa
├── Models/
│   ├── Kullanici.cs             # Kullanıcı entity
│   ├── Etkinlik.cs              # Etkinlik entity (QR alanları dahil)
│   ├── Fotograf.cs              # Medya dosyası entity
│   ├── EtkinlikViewModel.cs     # Dashboard için ViewModel
│   └── LoginViewModel.cs        # Giriş formu modeli
├── Views/
│   ├── Account/                 # Login, Kayıt, Profil, Şifre sayfaları
│   ├── Etkinlikler/             # Etkinlik listesi, ekleme, detay, yükleme
│   └── Shared/                  # Layout, validasyon
├── Migrations/                  # EF Core migration dosyaları
├── wwwroot/
│   └── uploads/                 # Yüklenen medyaların depolandığı klasör
├── QrPhotoDb.db                 # SQLite veritabanı
└── Program.cs                   # Uygulama başlangıç noktası
```

---

## 📸 Ekran Görüntüleri

> 💡 Uygulamayı başlattıktan sonra ekran görüntüleri buraya eklenebilir.

| Etkinlik Paneli | QR Kodu Sayfası | Medya Yükleme |
|:-:|:-:|:-:|
| *(screenshot)* | *(screenshot)* | *(screenshot)* |

---

## 🔮 Geliştirme Fikirleri

- [ ] Medya galerisini etkinlik bazında görüntüleme
- [ ] ZIP olarak toplu indirme
- [ ] Etkinlik için geçerlilik süresi belirleme
- [ ] QR kodunu PDF olarak indirme
- [ ] Yükleme sınırı (dosya boyutu & adet)
- [ ] Admin paneli

---

## 🤝 Katkı

Pull request'ler ve öneriler memnuniyetle karşılanır!

1. Fork edin (`git fork`)
2. Feature branch oluşturun (`git checkout -b feature/ozellik-adi`)
3. Commit edin (`git commit -m 'feat: yeni özellik'`)
4. Push edin (`git push origin feature/ozellik-adi`)
5. Pull Request açın

---

## 📄 Lisans

Bu proje [MIT](LICENSE) lisansı altında dağıtılmaktadır.

---

<div align="center">

**⭐ Projeyi faydalı bulduysan yıldız vermeyi unutma!**

*QR kod tarat, medyaları topla.* 📲

</div>
