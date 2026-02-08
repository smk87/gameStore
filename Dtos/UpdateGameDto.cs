using System.ComponentModel.DataAnnotations;

namespace GameStore.Dtos;

public record UpdateGameDto(
  [Required] [StringLength(50)] string Name,
  [Required] int GenreId,
  [Required] [Range(0, 100)] decimal Price,
  DateOnly ReleaseDate);