using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class MealIngredientCreateDto
{
    [Range(0, 9999999.99)]
    public decimal QuantityUsed { get; set; }

    [Range(1, int.MaxValue)]
    public int IngredientId { get; set; }
}
