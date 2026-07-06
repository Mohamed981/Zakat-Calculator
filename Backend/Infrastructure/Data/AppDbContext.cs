using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
	public virtual DbSet<Currency> Currencies => Set<Currency>();

	public virtual DbSet<Revenue> Revenues => Set<Revenue>();

	public virtual DbSet<Transaction> Transactions => Set<Transaction>();

	public virtual DbSet<User> Users => Set<User>();

	public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity(delegate(EntityTypeBuilder<Currency> entity)
		{
			entity.HasKey((Currency e) => e.CurrencyId).HasName("PK__currenci__C7F543D3F91FBD6D");
			entity.ToTable("currencies");
			entity.Property((Currency e) => e.CurrencyId).HasColumnName("currency_id");
			entity.Property((Currency e) => e.Code).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("code");
			entity.Property((Currency e) => e.Name).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("name");
			entity.Property((Currency e) => e.Rates).HasColumnType("text").HasColumnName("rates");
		});
		modelBuilder.Entity(delegate(EntityTypeBuilder<Revenue> entity)
		{
			entity.HasKey((Revenue e) => e.RevenueId).HasName("PK__revenues__3DF902E9083EAFE3");
			entity.ToTable("revenues");
			entity.Property((Revenue e) => e.RevenueId).HasColumnName("revenue_id");
			entity.Property((Revenue e) => e.CurrencyId).HasColumnName("currency_id");
			entity.Property((Revenue e) => e.UserId).HasColumnName("user_id");
			entity.Property((Revenue e) => e.Value).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("value");
			entity.Property((Revenue e) => e.ZakatIn21k).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("zakat_in21k");
			entity.Property((Revenue e) => e.ZakatPaid).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("zakat_paid");
			entity.Property((Revenue e) => e.ZakatRemaining).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("zakat_remaining");
			entity.HasOne((Revenue d) => d.Currency).WithMany((Currency p) => p.Revenues).HasForeignKey((Revenue d) => d.CurrencyId)
				.HasConstraintName("FKcs78soseex9j9p3vy0vr7j1ie");
			entity.HasOne((Revenue d) => d.User).WithMany((User p) => p.Revenues).HasForeignKey((Revenue d) => d.UserId)
				.HasConstraintName("FK2k0gkxfd3ar3b1btcguii1r");
		});
		modelBuilder.Entity(delegate(EntityTypeBuilder<Transaction> entity)
		{
			entity.HasKey((Transaction e) => e.TransactionId).HasName("PK__transact__85C600AFBE177ED8");
			entity.ToTable("transactions");
			entity.Property((Transaction e) => e.TransactionId).HasColumnName("transaction_id");
			entity.Property((Transaction e) => e.CurrencyId).HasColumnName("currency_id");
			entity.Property((Transaction e) => e.Date).HasPrecision(6).HasColumnName("date");
			entity.Property((Transaction e) => e.RevenueId).HasColumnName("revenue_id");
			entity.Property((Transaction e) => e.UserId).HasColumnName("user_id");
			entity.Property((Transaction e) => e.Value).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("value");
			entity.HasOne((Transaction d) => d.Currency).WithMany((Currency p) => p.Transactions).HasForeignKey((Transaction d) => d.CurrencyId)
				.HasConstraintName("FK812edr8o27pte306gvbmypytx");
			entity.HasOne((Transaction d) => d.Revenue).WithMany((Revenue p) => p.Transactions).HasForeignKey((Transaction d) => d.RevenueId)
				.HasConstraintName("FKj33777kxp1s38bxki5uoet0oq");
			entity.HasOne((Transaction d) => d.User).WithMany((User p) => p.Transactions).HasForeignKey((Transaction d) => d.UserId)
				.HasConstraintName("FKqwv7rmvc8va8rep7piikrojds");
		});
		modelBuilder.Entity(delegate(EntityTypeBuilder<User> entity)
		{
			entity.HasKey((User e) => e.UserId).HasName("PK__users__B9BE370FECC8DF55");
			entity.ToTable("users");
			entity.Property((User e) => e.UserId).HasColumnName("user_id");
			entity.Property((User e) => e.Name).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("name");
			entity.Property((User e) => e.TotalPaidZakatIn21k).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("total_paid_zakat_in21k");
			entity.Property((User e) => e.TotalZakatIn21k).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("total_zakat_in21k");
			entity.Property((User e) => e.ZakatDeadLine).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("zakat_dead_line");
			entity.Property((User e) => e.Provider).HasMaxLength(50).IsUnicode(unicode: false)
				.HasColumnName("provider");
			entity.Property((User e) => e.ExternalId).HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnName("external_id");
		});
	}
}
