using System.ComponentModel.DataAnnotations;

namespace GameStore.API.DTOs;

public record class CreateGameDTO(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(40)] string Genre,
    [Required][Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);
