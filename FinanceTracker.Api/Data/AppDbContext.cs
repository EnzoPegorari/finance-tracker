using FinanceTracker.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Name).HasMaxLength(100);
            entity.Property(u => u.Email).HasMaxLength(255);
            entity.Property(u => u.PasswordHash).HasMaxLength(255);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.Property(rt => rt.Token).HasMaxLength(500);
            entity.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(100);
            entity.Property(c => c.Color).HasMaxLength(7);
            entity.Property(c => c.Icon).HasMaxLength(50);
            entity.HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(t => t.Description).HasMaxLength(255);
            entity.Property(t => t.Amount).HasColumnType("decimal(18,2)");
            entity.Property(t => t.Type).HasMaxLength(10);
            entity.Property(t => t.Notes).HasMaxLength(500);
            entity.HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        SeedDefaultCategories(modelBuilder);
    }

    private static void SeedDefaultCategories(ModelBuilder modelBuilder)
    {
        var defaultCategories = new[]
        {
            new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111101"), UserId = null, Name = "Alimentação", Color = "#FF5733", Icon = "utensils", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111102"), UserId = null, Name = "Transporte", Color = "#33A1FF", Icon = "car", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111103"), UserId = null, Name = "Saúde", Color = "#33FF57", Icon = "heart-pulse", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111104"), UserId = null, Name = "Moradia", Color = "#A133FF", Icon = "home", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111105"), UserId = null, Name = "Educação", Color = "#FFC300", Icon = "book", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111106"), UserId = null, Name = "Lazer", Color = "#FF33A1", Icon = "gamepad", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111107"), UserId = null, Name = "Salário", Color = "#33FFC1", Icon = "wallet", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111108"), UserId = null, Name = "Outros", Color = "#808080", Icon = "shopping-cart", CreatedAt = DateTime.UtcNow },
        };

        modelBuilder.Entity<Category>().HasData(defaultCategories);
    }
}
