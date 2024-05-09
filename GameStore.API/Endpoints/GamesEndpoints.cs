using GameStore.API.Data;
using GameStore.API.DTOs;
using GameStore.API.Entities;
using GameStore.API.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    // Once I had the DB setup I removed the hardcoded list for testing
    // private static readonly List<GameSummaryDTO> games = [
    //         new (
    //             1,
    //             "World of Warcraft",
    //             "MMORPG",
    //             50.00M,
    //             new DateOnly(2004, 11, 14)
    //         ),
    //         new (
    //             2,
    //             "Final Fantasy XIV",
    //             "RPG",
    //             59.99M,
    //             new DateOnly(2010, 9, 30)
    //         ),
    //         new (
    //             3,
    //             "FIFA 23",
    //             "Sports",
    //             69.99M,
    //             new DateOnly(2022, 9, 27)
    //         )
    // ];

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
        // group.MapGet("/", () => games); // pre dbContext
        // ***** Add this to test slower connections *****
        // {
        //     // await Task.Delay(3000); 
        //     return await dbContext.Games
        //             .Include(game => game.Genre)
        //             .Select(game => game.ToGameSummaryDTO())
        //             .AsNoTracking() // don't need tracking by entity framework of the returned entities, just send them back to the client as this.  Improves performance when multiple entities are being returned
        //             .ToListAsync();
        // }
        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDTO())
                    .AsNoTracking() // don't need tracking by entity framework of the returned entities, just send them back to the client as this.  Improves performance when multiple entities are being returned
                    .ToListAsync()
        );


        // GET /games/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                // GameDTO? game = games.Find(game => game.Id == id); // without dbContext
                Game? game = await dbContext.Games.FindAsync(id);

                return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDTO());
            })
            .WithName(GetGameEndpointName);

        // POST /games
        // adding GameStoreContext dbContext
        // at runtime asp.core will handle resolving and providing an instance of dbContext
        group.MapPost("/", async (CreateGameDTO newGame, GameStoreContext dbContext) =>
        {
            //this would be needed before adding the dbContext
            // GameDTO game = new(
            //     games.Count + 1,
            //     newGame.Name,
            //     newGame.Genre,
            //     newGame.Price,
            //     newGame.ReleaseDate
            // );
            // games.Add(game);
            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            // don't return internal entities like "game"
            // return the Dto instead like "game.ToDTO()" 
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDTO());
        });

        // PUT /games
        group.MapPut("/{id}", async (int id, UpdateGameDTO updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                // You can also create a record if you want.  Many services do this, but for now I am returning not found.
                // If you create the record you can run into issues when dealing with databases.  
                // If you create an ID here it might not match the database and mess up
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
                    .CurrentValues
                    .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent(); // For most updates you are just returning no content
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            // games.RemoveAll(game => game.Id == id); // pre dbContext
            await dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
}
