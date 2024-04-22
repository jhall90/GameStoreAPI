using GameStore.API.DTOs;

namespace GameStore.API.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDTO> games = [
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

    // adding "this" to the parameters makes this an extension method
    // Creating a RouteGroupBuilder because all the routes below are using games/___
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                        .WithParameterValidation();
        // added MinimalApis.Extensions from nuget.org to easily add validation wrapper using .WithParameterValidation()
        // validation added to whole grouping but you can add it to each route separately

        // minimal API
        // GET /games
        group.MapGet("/", () => games);

        // GET /games/{id}
        group.MapGet("/{id}", (int id) =>
            {
                GameDTO? game = games.Find(game => game.Id == id);

                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDTO newGame) =>
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
        group.MapPut("/{id}", (int id, UpdateGameDTO updatedGame) =>
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
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}
