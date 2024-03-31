using GameStore.Api.Data;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenreSummary
{
    public static RouteGroupBuilder MapGenreEndpoints(this WebApplication app) {
        var group = app.MapGroup("genres");

        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                .Select(genre => genre.ToGenreSummaryDto())
                .AsNoTracking()
                .ToListAsync()
        );

        return group;
    }
}