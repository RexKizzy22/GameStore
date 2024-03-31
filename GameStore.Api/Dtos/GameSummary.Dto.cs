namespace GameStore.Api.Dto;
public record class GameSummaryDto(
    int Id, 
    string Name, 
    decimal Price, 
    string Genre, 
    DateOnly ReleaseDate
);
