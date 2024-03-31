using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtension {
    public static async Task MigrateDbAsync(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        var dBContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        await dBContext.Database.MigrateAsync();
    }
}
