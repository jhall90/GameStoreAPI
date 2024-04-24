// Records are immutable which is perfect for DTOs since they are usually carry data from point A to B without modification
// Records also reduce boilerplate code that classes normally contain
namespace GameStore.API.DTOs;

public record class GameSummaryDTO(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
