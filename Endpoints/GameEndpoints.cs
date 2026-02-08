using gameStore.Data;
using GameStore.Dtos;
using gameStore.Entities;
using gameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace gameStore.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/",
            (GameStoreContext dbContext) =>
            {
                return dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameDetailsDto());
            });

        // GET /games/{id}
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            Game? game = dbContext.Games
                .Include(game => game.Genre)
                .FirstOrDefault(game => game.Id == id);

            return game is not null ? Results.Ok(game.ToGameDetailsDto()) : Results.NotFound();
        }).WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genres.Find(newGame.GenreId);

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id },
                game.ToGameDetailsDto());
        });

        // PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            Game? existingGame = dbContext.Games.Find(id);


            if (existingGame is null)
            {
                return Results.NotFound();
            }

            var updatedGenre = dbContext.Genres.Find(updatedGame.GenreId);

            if (updatedGenre is null)
            {
                return Results.BadRequest("Invalid GenreId");
            }

            dbContext
                .Entry(existingGame)
                .CurrentValues
                .SetValues(updatedGame.ToEntity(id, updatedGenre));

            dbContext.SaveChanges();

            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {
            Game? game = dbContext.Games.Find(id);

            if (game is null)
            {
                return Results.NotFound();
            }

            dbContext.Games.Remove(game);
            dbContext.SaveChanges();

            return Results.NoContent();
        });

        return group;
    }
}