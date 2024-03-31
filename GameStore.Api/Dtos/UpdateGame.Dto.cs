using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dto;
public record class UpdateGameDto(
    [Required][StringLength(50)] string Name, 
    [Range(1, 100)] decimal Price, 
    int GenreId, 
    DateOnly ReleaseDate
);
