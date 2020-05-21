using System.Collections;
using System.Collections.Generic;
using CurrencyExchange.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.DAL
{
    public class CurrencyExchangeDbContext: DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<HistoryRow> History { get; set; }
        
        public CurrencyExchangeDbContext(DbContextOptions options): base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.CurrencyShortName)
                    .HasName("PK_Currencies_CurrencyShortName");

                entity.Property(e => e.CurrencyShortName).HasMaxLength(10);

                entity.Property(e => e.CountryShortName)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<HistoryRow>(entity =>
            {
                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FromAmount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.FromCurrency)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ToAmount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.ToCurrency)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.FromCurrencyNavigation)
                    .WithMany(p => p.HistoryFromCurrencyNavigation)
                    .HasForeignKey(d => d.FromCurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_History_FromCurrency");

                entity.HasOne(d => d.ToCurrencyNavigation)
                    .WithMany(p => p.HistoryToCurrencyNavigation)
                    .HasForeignKey(d => d.ToCurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_History_ToCurrency");
            });
        }
    }
    
    
}