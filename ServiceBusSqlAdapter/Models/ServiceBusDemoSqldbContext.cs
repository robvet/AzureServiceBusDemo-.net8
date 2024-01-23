using Microsoft.EntityFrameworkCore;

namespace ServiceBusSqlAdapter.Models;

public partial class ServiceBusDemoSqldbContext : DbContext
{
    public ServiceBusDemoSqldbContext()
    {
    }

    public ServiceBusDemoSqldbContext(DbContextOptions<ServiceBusDemoSqldbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Billing> Billings { get; set; }

    public virtual DbSet<Claim> Claims { get; set; }

    public virtual DbSet<TransactionLog> TransactionLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Billing>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.BillingNumber)
                .HasMaxLength(50)
                .HasColumnName("billing-number");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create-date");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Insured)
                .HasMaxLength(50)
                .HasColumnName("insured");
        });

        modelBuilder.Entity<Claim>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ClaimNumber)
                .HasMaxLength(50)
                .HasColumnName("claim-number");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create-date");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Insured)
                .HasMaxLength(50)
                .HasColumnName("insured");
        });

        modelBuilder.Entity<TransactionLog>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("create-date");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Insured)
                .HasMaxLength(50)
                .HasColumnName("insured");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
