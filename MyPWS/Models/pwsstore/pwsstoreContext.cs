using Microsoft.EntityFrameworkCore;


namespace MyPWS.Models.pwsstore
{
    public partial class PwsStoreContext : Microsoft.EntityFrameworkCore.DbContext
    {   
        public PwsStoreContext(DbContextOptions<PwsStoreContext> options)
            : base(options)
        {
            
        }

        public virtual Microsoft.EntityFrameworkCore.DbSet<Configwunderground> Configwunderground { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<Pws> Pws { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<Weather> Weather { get; set; }     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Weather>().Property(p => p.Tempc).HasPrecision(3, 1);
            modelBuilder.Entity<Weather>().Property(p => p.Humidity).HasPrecision(3);
            modelBuilder.Entity<Weather>().Property(p => p.Dewptc).HasPrecision(3, 1);
            modelBuilder.Entity<Weather>().Property(p => p.Baromhpa).HasPrecision(6, 2);
            modelBuilder.Entity<Weather>().Property(p => p.Winddir).HasPrecision(3);
            modelBuilder.Entity<Weather>().Property(p => p.Windspeedkmh).HasPrecision(4,1);
            modelBuilder.Entity<Weather>().Property(p => p.Windgustkmh).HasPrecision(4, 1);
            modelBuilder.Entity<Weather>().Property(p => p.Uv).HasPrecision(3, 1);
            modelBuilder.Entity<Weather>().Property(p => p.Rainmm).HasPrecision(6,2);
            modelBuilder.Entity<Weather>().Property(p => p.Dailyrainmm).HasPrecision(6, 2);
            modelBuilder.Entity<Weather>().Property(p => p.Indoortempc).HasPrecision(3, 1);
            modelBuilder.Entity<Weather>().Property(p => p.Indoorhumidity).HasPrecision(3);

        }

    }
}
