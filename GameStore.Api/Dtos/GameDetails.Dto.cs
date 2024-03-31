namespace GameStore.Api.Dto;
public record class GameDetailsDto(
    int Id, 
    string Name, 
    decimal Price, 
    int GenreId, 
    DateOnly ReleaseDate
);
