﻿using GameStore.API.DTOs;
using GameStore.API.Entities;

namespace GameStore.API.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameDTO game)
    {
        return new Game()
        {
            Name = game.Name,
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

    public static Game ToEntity(this UpdateGameDTO game, int id)
    {
        return new Game()
        {
            Id = id,
            Name = game.Name,
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

    public static GameSummaryDTO ToGameSummaryDTO(this Game game)
    {
        return new(
            game.Id,
            game.Name,
            game.Genre!.Name, // ! is a null operator that says we know it will never be null at this point
            game.Price,
            game.ReleaseDate
        );
    }

    public static GameDetailsDTO ToGameDetailsDTO(this Game game)
    {
        return new(
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate
        );
    }
}
