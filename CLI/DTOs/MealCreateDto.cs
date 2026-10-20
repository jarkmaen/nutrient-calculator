namespace CLI.DTOs;

public class MealCreateDto
{
    public int Servings { get; set; }
    public List<MealIngredientCreateDto> MealIngredients { get; set; } = [];
    public string Name { get; set; } = string.Empty;
}
