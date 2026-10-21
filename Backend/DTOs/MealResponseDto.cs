using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class MealResponseDto
{
    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Id { get; set; }

    [Required]
    public int Servings { get; set; }

    [Required]
    public List<MealIngredientResponseDto> MealIngredients { get; set; } = [];

    [Required]
    public NutrientDto Nutrients { get; set; } = new();

    [Required]
    public string Name { get; set; } = string.Empty;
}
