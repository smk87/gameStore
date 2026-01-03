using gameStore.Data;
using GameStore.Dtos;
using gameStore.Entities;

namespace gameStore.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> Games =
    [
        new(1, "Game 1", "Genre 1", 10.0m, new DateOnly(2021, 1, 1)),
        new(2, "Game 2", "Genre 2", 20.0m, new DateOnly(2022, 2, 2)),
        new(3, "Game 3", "Genre 3", 30.0m, new DateOnly(2023, 3, 3))
    ];

    public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/", () => Games);

        // GET /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = Games.Find(game => game.Id == id);

            return game is not null ? Results.Ok(game) : Results.NotFound();
        }).WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                Genre = dbContext.Genres.Find(newGame.GenreId),
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            GameDto gameDto = new(game.Id, game.Name, game.Genre!.Name, game.Price, game.ReleaseDate);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, gameDto);
        });

        // PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = Games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            Games[index] = new GameDto(id, updatedGame.Name, updatedGame.Genre, updatedGame.Price,
                updatedGame.ReleaseDate);

            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            Games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}