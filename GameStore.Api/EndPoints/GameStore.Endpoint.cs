using System.Security.Claims;
using GameStore.Api.Dto;
using GameStore.Api.Mapping;
using GameStore.Api.Repository;


namespace GameStore.Api.Endpoints;

public static class Endpoints {
    const string GetGameEndpoint = "GetGame";

    public static RouteGroupBuilder MapGameStoreEndpoints(this WebApplication app) {

        var scope = app.Services.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<GameStoreRepository>();

        var group = app.MapGroup("games")
                        .WithParameterValidation()
                        .RequireAuthorization(policy => policy.RequireRole("admin"));

        group.MapGet("/", async () => await repo.GetAllGamesAsync());

        group.MapGet("/{id}", async (int id) => {
            var game = await repo.GetOneGameAsync(id);

            return game is null 
                    ? Results.NotFound() 
                    : Results.Ok(game.ToGameDetailsDto());

        }).WithName(GetGameEndpoint);

        group.MapPost("/create", async (CreateGameDto newGame, ClaimsPrincipal user) => {
            ArgumentNullException.ThrowIfNull(user.Identity?.Name);
            var username = user.Identity.Name;
            
           var game = await repo.CreateGameAsync(newGame);

            return Results.CreatedAtRoute(
                GetGameEndpoint, 
                new { id = game.Id }, game
            );
        });

        group.MapPut("/{id}/update", async (int id, UpdateGameDto updatedGame) => {
            var savedGame = await repo.UpdateGameAsync(id, updatedGame);

            if (savedGame is null) return Results.NotFound();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id) =>
        {
            var isSuccessful = await repo.DeleteGameAsync(id);

            return !isSuccessful ? Results.NotFound() : Results.NoContent();
        });

        return group;
    }

}
