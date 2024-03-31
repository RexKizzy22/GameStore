using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options): DbContext(options) {
    public DbSet<GameEntity> Games => Set<GameEntity>();

    public DbSet<GenreEntity> Genres => Set<GenreEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GenreEntity>().HasData(
            new { Id = 1, Name = "Fighting" },
            new { Id = 2, Name = "RolePlaying" },
            new { Id = 3, Name = "Sports" },
            new { Id = 4, Name = "Racing" },
            new { Id = 5, Name = "Kids and Family" }
        );
    }
}
