namespace GameStore.Api.Repository;

using Microsoft.EntityFrameworkCore;

using GameStore.Api.Data;
using GameStore.Api.Dto;
using GameStore.Api.Mapping;
using GameStore.Api.Entities;


interface IGameStoreRepository {
    Task<List<GameSummaryDto>> GetAllGamesAsync();
    Task<GameEntity?> GetOneGameAsync(int id);
    Task<GameSummaryDto> CreateGameAsync(CreateGameDto newGame);
    Task<GameEntity?> UpdateGameAsync(int id, UpdateGameDto updatedGame);
    Task<bool> DeleteGameAsync(int id);
}

public class GameStoreRepository : IGameStoreRepository {

    private readonly ILogger<GameStoreRepository> _logger;
    private readonly GameStoreContext _gameStoreContext;

    public GameStoreRepository(ILogger<GameStoreRepository> logger, GameStoreContext gameStoreContext)
    {
        this._logger = logger;
        this._gameStoreContext = gameStoreContext;
    }

    public async Task<List<GameSummaryDto>> GetAllGamesAsync() {
        return await _gameStoreContext.Games
                        .Include(game => game.Genre)
                        .Select(game => game.ToGameSummaryDto())
                        .AsNoTracking()
                        .ToListAsync();
    }

    public async Task<GameEntity?> GetOneGameAsync(int id) {
        return await _gameStoreContext.Games.FindAsync(id);
    }

    public async Task<GameSummaryDto> CreateGameAsync(CreateGameDto newGame) {
        GameEntity game = newGame.ToEntity();
        game.Genre = await _gameStoreContext.Genres.FindAsync(game.GenreId);

        _gameStoreContext.Games.Add(game);
        var numOfGamesCreated = await _gameStoreContext.SaveChangesAsync();

        if (Convert.ToBoolean(numOfGamesCreated)) {
            _logger.LogInformation("Successfully created game with id {Id}", game.Id);
        } else {
            _logger.LogError("Failed attempt to create game - {Name}", game.Name);
        }

        return game.ToGameSummaryDto();
    }

    public async Task<GameEntity?> UpdateGameAsync(int id, UpdateGameDto updatedGame) {
        var existingGame = await _gameStoreContext.Games.FindAsync(id);

        if (existingGame is null) {
            _logger.LogError("Attempted to update non-existing game of id {Id}", id);
            return null;
        };

        var savedGame = updatedGame.ToEntity(id);

        _gameStoreContext
            .Entry(existingGame)
            .CurrentValues
            .SetValues(savedGame);

        await _gameStoreContext.SaveChangesAsync();

        _logger.LogInformation("Successfully updated game {Name} of id {Id}", savedGame.Name, savedGame.Id);

        return savedGame;
    }

    public async Task<bool> DeleteGameAsync(int id) {
        int numOfRowsDeleted = await _gameStoreContext.Games
                        .Where(game => game.Id == id)
                        .ExecuteDeleteAsync();

        if (numOfRowsDeleted == 0) { 
            _logger.LogError("Attempted to deleted non-existing game of id {Id}", id);
            return false;
        }

        _logger.LogInformation("Successfully deleted game with id {Id}", id);

        return true;
    }
}