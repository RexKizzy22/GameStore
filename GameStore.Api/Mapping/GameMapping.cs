using GameStore.Api.Dto;
using GameStore.Api.Entities;

namespace GameStore.Api.Mapping;

public static class Mapping {
    public static GameEntity ToEntity(this CreateGameDto gameDto) {
        return new GameEntity()
        {
            Name = gameDto.Name,
            Price = gameDto.Price,
            GenreId = gameDto.GenreId,
            ReleaseDate = gameDto.ReleaseDate
        };
     }
    public static GameEntity ToEntity(this UpdateGameDto gameDto, int id) {
        return new GameEntity()
        {
            Id = id,
            Name = gameDto.Name,
            Price = gameDto.Price,
            GenreId = gameDto.GenreId,
            ReleaseDate = gameDto.ReleaseDate
        };
     }

    public static GameSummaryDto ToGameSummaryDto(this GameEntity gameEntity) { 
        return new (
                gameEntity.Id,
                gameEntity.Name,
                gameEntity.Price,
                gameEntity.Genre!.Name,
                gameEntity.ReleaseDate
            );
    }
    public static GameDetailsDto ToGameDetailsDto(this GameEntity gameEntity) { 
        return new (
                gameEntity.Id,
                gameEntity.Name,
                gameEntity.Price,
                gameEntity.GenreId,
                gameEntity.ReleaseDate
            );
    }
    public static GenreDto ToGenreSummaryDto(this GenreEntity genreEntity) { 
        return new (
                genreEntity.Id,
                genreEntity.Name
            );
    }
}