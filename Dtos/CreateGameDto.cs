using System.ComponentModel.DataAnnotations;

namespace GameStore.Dtos;

public record CreateGameDto(
    [Required] [StringLength(50)] string Name,
    [Required] [StringLength(20)] string Genre,
    [Required] [Range(0, 100)] decimal Price,
    DateOnly ReleaseDate);