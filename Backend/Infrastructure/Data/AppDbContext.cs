using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public virtual DbSet<Currency> Currencies => Set<Currency>();

    public virtual DbSet<Revenue> Revenues => Set<Revenue>();

    public virtual DbSet<Transaction> Transactions => Set<Transaction>();

    public virtual DbSet<User> Users => Set<User>();

    //public AppDbContext(DbContextOptions<AppDbContext> options)
    //    : base(options)
    //{
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasName("PK__currenci__C7F543D3F91FBD6D");

            entity.ToTable("currencies");

            entity.Property(e => e.CurrencyId).HasColumnName("currency_id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Rates)
                .HasColumnType("text")
                .HasColumnName("rates");
        });

        modelBuilder.Entity<Revenue>(entity =>
        {
            entity.HasKey(e => e.RevenueId).HasName("PK__revenues__3DF902E9083EAFE3");

            entity.ToTable("revenues");

            entity.Property(e => e.RevenueId).HasColumnName("revenue_id");
            entity.Property(e => e.CurrencyId).HasColumnName("currency_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("value");
            entity.Property(e => e.ZakatIn21k)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("zakat_in21k");
            entity.Property(e => e.ZakatPaid)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("zakat_paid");
            entity.Property(e => e.ZakatRemaining)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("zakat_remaining");

            entity.HasOne(d => d.Currency).WithMany(p => p.Revenues)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FKcs78soseex9j9p3vy0vr7j1ie");

            entity.HasOne(d => d.User).WithMany(p => p.Revenues)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK2k0gkxfd3ar3b1btcguii1r");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__transact__85C600AFBE177ED8");

            entity.ToTable("transactions");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.CurrencyId).HasColumnName("currency_id");
            entity.Property(e => e.Date)
                .HasPrecision(6)
                .HasColumnName("date");
            entity.Property(e => e.RevenueId).HasColumnName("revenue_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Value)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("value");

            entity.HasOne(d => d.Currency).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK812edr8o27pte306gvbmypytx");

            entity.HasOne(d => d.Revenue).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.RevenueId)
                .HasConstraintName("FKj33777kxp1s38bxki5uoet0oq");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FKqwv7rmvc8va8rep7piikrojds");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370FECC8DF55");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.ExternalId).HasMaxLength(100);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Provider).HasMaxLength(100);
            entity.Property(e => e.TotalPaidZakatIn21k)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("total_paid_zakat_in21k");
            entity.Property(e => e.TotalZakatIn21k)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("total_zakat_in21k");
            entity.Property(e => e.ZakatDeadLine)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("zakat_dead_line");
        });
    }
}
