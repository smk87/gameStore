using gameStore.Entities;

namespace GameStore.Dtos;

public record GameDetailsDto(int Id, string Name, string Genre, decimal Price, DateOnly ReleaseDate);

