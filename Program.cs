using GameStore.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
const string getGameEndpointName = "GetGame";

List<GameDto> games =
[
    new(1, "Game 1", "Genre 1", 10.0m, new DateOnly(2021, 1, 1)),
    new(2, "Game 2", "Genre 2", 20.0m, new DateOnly(2022, 2, 2)),
    new(3, "Game 3", "Genre 3", 30.0m, new DateOnly(2023, 3, 3))
];

// GET /games
app.MapGet("/games", () => games);

// GET /games/{id}
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id)).WithName(getGameEndpointName);

// POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto gameToCreate = new(games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);

    games.Add(gameToCreate);

    return Results.CreatedAtRoute(getGameEndpointName, new { id = gameToCreate.Id }, gameToCreate);
});

// PUT /games/{id}
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    games[index] = new GameDto(id, updatedGame.Name, updatedGame.Genre, updatedGame.Price, updatedGame.ReleaseDate);

    return Results.NoContent();
});

// DELETE /games/{id}
app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.Run();
