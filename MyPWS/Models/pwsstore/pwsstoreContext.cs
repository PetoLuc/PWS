using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace MyPWS.Models.pwsstore
{
    public partial class pwsstoreContext : DbContext
    {
        public pwsstoreContext()
        {
        }

        public pwsstoreContext(DbContextOptions<pwsstoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Configwunderground> Configwunderground { get; set; }
        public virtual DbSet<Pws> Pws { get; set; }
        public virtual DbSet<Weather> Weather { get; set; }
  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configwunderground>(entity =>
            {
                entity.ToTable("configwunderground");

                entity.HasIndex(e => e.Id)
                    .HasName("idPWSUpload_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IdPws)
                    .HasName("pws_idx");

                entity.HasIndex(e => e.Wuid)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.Desc)
                    .HasColumnName("desc")
                    .HasColumnType("longtext");

                entity.Property(e => e.IdPws)
                    .HasColumnName("idPWS")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasColumnName("pwd")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Wuid)
                    .IsRequired()
                    .HasColumnName("wuid")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPwsNavigation)
                    .WithMany(p => p.Configwunderground)
                    .HasForeignKey(d => d.IdPws)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pws0");
            });

            modelBuilder.Entity<Pws>(entity =>
            {
                entity.HasKey(e => e.IdPws)
                    .HasName("PRIMARY");

                entity.ToTable("pws");

                entity.HasIndex(e => e.Id)
                    .HasName("ID_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IdPws)
                    .HasName("idPWS_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdPws)
                    .HasColumnName("idPWS")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Alt).HasColumnName("alt");

                entity.Property(e => e.Desc)
                    .HasColumnName("desc")
                    .HasColumnType("longtext");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Lat)
                    .HasColumnName("lat")
                    .HasColumnType("decimal(10,8)");

                entity.Property(e => e.Lon)
                    .HasColumnName("lon")
                    .HasColumnType("decimal(11,8)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasColumnName("pwd")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Weather>(entity =>
            {
                entity.ToTable("weather");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IdPws)
                    .HasName("pws_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.Baromhpa)
                    .HasColumnName("baromhpa")
                    .HasColumnType("decimal(7,3) unsigned");

                entity.Property(e => e.Dailyrainmm)
                    .HasColumnName("dailyrainmm")
                    .HasColumnType("decimal(5,2) unsigned");

                entity.Property(e => e.Dateutc)
                    .HasColumnName("dateutc")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Dewptc)
                    .HasColumnName("dewptc")
                    .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Humidity)
                    .HasColumnName("humidity")
                    .HasColumnType("smallint unsigned");

                entity.Property(e => e.IdPws)
                    .HasColumnName("idPWS")
                    .HasColumnType("int unsigned");

                entity.Property(e => e.Indoorhumidity)
                    .HasColumnName("indoorhumidity")
                    .HasColumnType("smallint unsigned");

                entity.Property(e => e.Indoortempc)
                    .HasColumnName("indoortempc")
                    .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Rainmm)
                    .HasColumnName("rainmm")
                    .HasColumnType("decimal(5,2) unsigned");

                entity.Property(e => e.Tempc)
                    .HasColumnName("tempc")
                    .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Uv)
                    .HasColumnName("uv")
                    .HasColumnType("decimal(5,2) unsigned");

                entity.Property(e => e.Winddir)
                    .HasColumnName("winddir")
                    .HasColumnType("smallint unsigned");

                entity.Property(e => e.Windgustdir)
                    .HasColumnName("windgustdir")
                    .HasColumnType("smallint unsigned");

                entity.Property(e => e.Windgustkmh)
                    .HasColumnName("windgustkmh")
                    .HasColumnType("decimal(5,2) unsigned");

                entity.Property(e => e.Windspeedkmh)
                    .HasColumnName("windspeedkmh")
                    .HasColumnType("decimal(5,2) unsigned");

                entity.HasOne(d => d.IdPwsNavigation)
                    .WithMany(p => p.Weather)
                    .HasForeignKey(d => d.IdPws)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pws1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
