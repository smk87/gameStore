namespace GameStore.Dtos;

public record UpdateGameDto(int Id, string Name, string Genre, decimal Price, DateOnly ReleaseDate);