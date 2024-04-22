namespace GameStore.API.Entities;

public class Game
{
    public int Id { get; set; }
    public required string Name { get; set; }

    // When you want a 1-1 relationship between Game and Genre Entity Classes 
    // you do what I did with the GenreId and Genre props below
    public int GenreId { get; set; }
    public Genre? Genre { get; set; } // Sometimes this will be null so I am adding the ? nullable
    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
}
