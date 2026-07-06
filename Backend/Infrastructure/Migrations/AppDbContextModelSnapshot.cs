using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
internal class AppDbContextModelSnapshot : ModelSnapshot
{
	protected override void BuildModel(ModelBuilder modelBuilder)
	{
		modelBuilder.HasAnnotation("ProductVersion", "10.0.8").HasAnnotation("Relational:MaxIdentifierLength", 128);
		modelBuilder.UseIdentityColumns(1L);
		modelBuilder.Entity("Domain.Entities.Currency", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("CurrencyId").ValueGeneratedOnAdd().HasColumnType("int")
				.HasColumnName("currency_id");
			b.Property<int>("CurrencyId").UseIdentityColumn(1L);
			b.Property<string>("Code").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("code");
			b.Property<string>("Name").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("name");
			b.Property<string>("Rates").HasColumnType("text").HasColumnName("rates");
			b.HasKey("CurrencyId").HasName("PK__currenci__C7F543D3F91FBD6D");
			b.ToTable("currencies", (string?)null);
		});
		modelBuilder.Entity("Domain.Entities.Revenue", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("RevenueId").ValueGeneratedOnAdd().HasColumnType("int")
				.HasColumnName("revenue_id");
			b.Property<int>("RevenueId").UseIdentityColumn(1L);
			b.Property<int?>("CurrencyId").HasColumnType("int").HasColumnName("currency_id");
			b.Property<int?>("UserId").HasColumnType("int").HasColumnName("user_id");
			b.Property<string>("Value").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("value");
			b.Property<string>("ZakatIn21k").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("zakat_in21k");
			b.Property<string>("ZakatPaid").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("zakat_paid");
			b.Property<string>("ZakatRemaining").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("zakat_remaining");
			b.HasKey("RevenueId").HasName("PK__revenues__3DF902E9083EAFE3");
			b.HasIndex("CurrencyId");
			b.HasIndex("UserId");
			b.ToTable("revenues", (string?)null);
		});
		modelBuilder.Entity("Domain.Entities.Transaction", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("TransactionId").ValueGeneratedOnAdd().HasColumnType("int")
				.HasColumnName("transaction_id");
			b.Property<int>("TransactionId").UseIdentityColumn(1L);
			b.Property<int?>("CurrencyId").HasColumnType("int").HasColumnName("currency_id");
			b.Property<DateTime?>("Date").HasPrecision(6).HasColumnType("datetime2(6)")
				.HasColumnName("date");
			b.Property<int?>("RevenueId").HasColumnType("int").HasColumnName("revenue_id");
			b.Property<int?>("UserId").HasColumnType("int").HasColumnName("user_id");
			b.Property<string>("Value").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("value");
			b.HasKey("TransactionId").HasName("PK__transact__85C600AFBE177ED8");
			b.HasIndex("CurrencyId");
			b.HasIndex("RevenueId");
			b.HasIndex("UserId");
			b.ToTable("transactions", (string?)null);
		});
		modelBuilder.Entity("Domain.Entities.User", delegate(EntityTypeBuilder b)
		{
			b.Property<int>("UserId").ValueGeneratedOnAdd().HasColumnType("int")
				.HasColumnName("user_id");
			b.Property<int>("UserId").UseIdentityColumn(1L);
			b.Property<string>("Email").HasColumnType("nvarchar(max)");
			b.Property<string>("ExternalId").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("external_id");
			b.Property<string>("Name").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("name");
			b.Property<string>("Password").HasColumnType("nvarchar(max)");
			b.Property<string>("Provider").HasMaxLength(50).IsUnicode(unicode: false)
				.HasColumnType("varchar(50)")
				.HasColumnName("provider");
			b.Property<string>("TotalPaidZakatIn21k").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("total_paid_zakat_in21k");
			b.Property<string>("TotalZakatIn21k").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("total_zakat_in21k");
			b.Property<string>("ZakatDeadLine").HasMaxLength(255).IsUnicode(unicode: false)
				.HasColumnType("varchar(255)")
				.HasColumnName("zakat_dead_line");
			b.HasKey("UserId").HasName("PK__users__B9BE370FECC8DF55");
			b.ToTable("users", (string?)null);
		});
		modelBuilder.Entity("Domain.Entities.Revenue", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Domain.Entities.Currency", "Currency").WithMany("Revenues").HasForeignKey("CurrencyId")
				.HasConstraintName("FKcs78soseex9j9p3vy0vr7j1ie");
			b.HasOne("Domain.Entities.User", "User").WithMany("Revenues").HasForeignKey("UserId")
				.HasConstraintName("FK2k0gkxfd3ar3b1btcguii1r");
			b.Navigation("Currency");
			b.Navigation("User");
		});
		modelBuilder.Entity("Domain.Entities.Transaction", delegate(EntityTypeBuilder b)
		{
			b.HasOne("Domain.Entities.Currency", "Currency").WithMany("Transactions").HasForeignKey("CurrencyId")
				.HasConstraintName("FK812edr8o27pte306gvbmypytx");
			b.HasOne("Domain.Entities.Revenue", "Revenue").WithMany("Transactions").HasForeignKey("RevenueId")
				.HasConstraintName("FKj33777kxp1s38bxki5uoet0oq");
			b.HasOne("Domain.Entities.User", "User").WithMany("Transactions").HasForeignKey("UserId")
				.HasConstraintName("FKqwv7rmvc8va8rep7piikrojds");
			b.Navigation("Currency");
			b.Navigation("Revenue");
			b.Navigation("User");
		});
		modelBuilder.Entity("Domain.Entities.Currency", delegate(EntityTypeBuilder b)
		{
			b.Navigation("Revenues");
			b.Navigation("Transactions");
		});
		modelBuilder.Entity("Domain.Entities.Revenue", delegate(EntityTypeBuilder b)
		{
			b.Navigation("Transactions");
		});
		modelBuilder.Entity("Domain.Entities.User", delegate(EntityTypeBuilder b)
		{
			b.Navigation("Revenues");
			b.Navigation("Transactions");
		});
	}
}
