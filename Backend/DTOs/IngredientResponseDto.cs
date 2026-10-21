using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class IngredientResponseDto
{
    [Required]
    public decimal Price { get; set; }

    [Required]
    public decimal Quantity { get; set; }

    [Required]
    public int Id { get; set; }

    [Required]
    public int? UsdaId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
}
