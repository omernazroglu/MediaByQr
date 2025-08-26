using MediaByQr.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MediaByQr
{
    public class ApplicationDbContext:Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Etkinlik> Etkinlikler { get; set; }
        public DbSet<Fotograf> Fotograflar { get; set; }
    }
}

