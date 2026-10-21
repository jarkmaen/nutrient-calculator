using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class MealCreateDto
{
    [Range(1, int.MaxValue)]
    public int Servings { get; set; }

    [MinLength(1)]
    public List<MealIngredientCreateDto> MealIngredients { get; set; } = [];

    [MaxLength(255)]
    [Required]
    public string Name { get; set; } = string.Empty;
}
