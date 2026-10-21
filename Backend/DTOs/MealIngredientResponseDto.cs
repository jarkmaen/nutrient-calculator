using Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class MealIngredientResponseDto
{
    [Required]
    public decimal QuantityUsed { get; set; }

    [Required]
    public Ingredient Ingredient { get; set; } = null!;
}
