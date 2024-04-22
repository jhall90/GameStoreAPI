using GameStore.API.DTOs;

var builder = WebApplication.CreateBuilder(args);
//add builder configurations before the var app

var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDTO> games = [
        new (
            1,
            "World of Warcraft",
            "MMORPG",
            50.00M,
            new DateOnly(2004, 11, 14)
        ),
        new (
            2,
            "Final Fantasy XIV",
            "RPG",
            59.99M,
            new DateOnly(2010, 9, 30)
        ),
        new (
            3,
            "FIFA 23",
            "Sports",
            69.99M,
            new DateOnly(2022, 9, 27)
        )
];

// minimal API
// GET /games
app.MapGet("games", () => games); 

// GET /games/{id}
app.MapGet("games/{id}", (int id) => 
    {
        GameDTO? game = games.Find(game => game.Id == id); 

        return game is null ? Results.NotFound() : Results.Ok(game);
    })
    .WithName(GetGameEndpointName);

// POST /games
app.MapPost("games", (CreateGameDTO newGame) => 
{
    GameDTO game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

// PUT /games
app.MapPut("games/{id}", (int id, UpdateGameDTO updatedGame) => 
{
    var index = games.FindIndex(game => game.Id == id);

    if (index == -1)
    {
        // You can also create a record if you want.  Many services do this, but for now I am returning not found.
        // If you create the record you can run into issues when dealing with databases.  
        // If you create an ID here it might not match the database and mess up
        return Results.NotFound(); 
    }

    games[index] = new GameDTO(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent(); // For most updates you are just returning no content
});

// DELETE /games/{id}
app.MapDelete("games/{id}", (int id) => 
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.Run();
