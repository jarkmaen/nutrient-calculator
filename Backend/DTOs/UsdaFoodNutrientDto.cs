namespace Backend.DTOs;

public class UsdaFoodNutrientDto
{
    public decimal Amount { get; set; }
    public UsdaNutrientDto Nutrient { get; set; } = null!;
}
