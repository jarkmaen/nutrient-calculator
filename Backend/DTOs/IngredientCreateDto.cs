using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class IngredientCreateDto
{
    [Range(0, 9999999.99)]
    public decimal Carbohydrate { get; set; }

    [Range(0, 9999999.99)]
    public decimal Energy { get; set; }

    [Range(0, 9999999.99)]
    public decimal Fat { get; set; }

    [Range(0, 9999999.99)]
    public decimal Fiber { get; set; }

    [Range(0, 9999999.99)]
    public decimal Price { get; set; }

    [Range(0, 9999999.99)]
    public decimal Protein { get; set; }

    [Range(0, 9999999.99, MinimumIsExclusive = true)]
    public decimal Quantity { get; set; }

    [Range(0, 9999999.99)]
    public decimal Salt { get; set; }

    [Range(0, 9999999.99)]
    public decimal SaturatedFat { get; set; }

    [Range(0, 9999999.99)]
    public decimal Sugar { get; set; }

    [Range(1, int.MaxValue)]
    public int? UsdaId { get; set; }

    [MaxLength(255)]
    [Required]
    public string Name { get; set; } = string.Empty;
}
