using GameStore.Dtos;
using gameStore.Entities;

namespace gameStore.Mapping;

public static class GameMapping
{
  public static Game ToEntity(this CreateGameDto game)
  {
    return new Game()
    {
      Name = game.Name,
      GenreId = game.GenreId,
      Price = game.Price,
      ReleaseDate = game.ReleaseDate
    };
  }

  public static Game ToEntity(this UpdateGameDto game, int id, Genre genre)
  {
    return new Game()
    {
      Id = id,
      Name = game.Name,
      GenreId = genre.Id,
      Genre = genre,
      Price = game.Price,
      ReleaseDate = game.ReleaseDate
    };
  }

  public static GameDetailsDto ToGameDetailsDto(this Game game)
  {
    return new GameDetailsDto(game.Id, game.Name, game.Genre!.Name, game.Price, game.ReleaseDate);
  }
}