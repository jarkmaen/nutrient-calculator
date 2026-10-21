using Backend.Data;
using Backend.DTOs;
using Backend.Mappers;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class MealService(AppDbContext context) : IMealService
{
    public async Task<List<MealResponseDto>> GetAllAsync()
    {
        var meals = await context.Meals
            .Include(m => m.MealIngredients).ThenInclude(mI => mI.Ingredient).ThenInclude(i => i.Macro)
            .Include(m => m.MealIngredients).ThenInclude(mI => mI.Ingredient).ThenInclude(i => i.Mineral)
            .Include(m => m.MealIngredients).ThenInclude(mI => mI.Ingredient).ThenInclude(i => i.Vitamin)
            .ToListAsync();

        return [.. meals.Select(ToResponseDto)];
    }

    public async Task<MealResponseDto?> AddAsync(MealCreateDto dto)
    {
        var ingredientIds = dto.MealIngredients.Select(i => i.IngredientId).ToList();

        int existingCount = await context.Ingredients.CountAsync(i => ingredientIds.Contains(i.Id));

        if (existingCount != ingredientIds.Count)
        {
            return null;
        }

        var meal = new Meal
        {
            Name = dto.Name,
            Servings = dto.Servings,
            MealIngredients = [.. dto.MealIngredients.Select(i => new MealIngredient
            {
                IngredientId = i.IngredientId,
                QuantityUsed = i.QuantityUsed
            })]
        };

        context.Meals.Add(meal);
        await context.SaveChangesAsync();

        return (await GetByIdAsync(meal.Id))!;
    }

    public async Task<MealResponseDto?> GetByIdAsync(int id)
    {
        var meal = await context.Meals
            .Include(m => m.MealIngredients).ThenInclude(mI => mI.Ingredient).ThenInclude(i => i.Macro)
            .Include(m => m.MealIngredients).ThenInclude(mI => mI.Ingredient).ThenInclude(i => i.Mineral)
            .Include(m => m.MealIngredients).ThenInclude(mI => mI.Ingredient).ThenInclude(i => i.Vitamin)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (meal == null)
        {
            return null;
        }

        return ToResponseDto(meal);
    }

    private static decimal CalculatePrice(Meal meal)
    {
        decimal total = meal.MealIngredients
            .Where(mI => mI.Ingredient.Quantity > 0)
            .Sum(mI => mI.Ingredient.Price * mI.QuantityUsed / mI.Ingredient.Quantity);

        return Math.Round(total / meal.Servings, 2);
    }

    private static MealResponseDto ToResponseDto(Meal meal)
    {
        return new MealResponseDto
        {
            Price = CalculatePrice(meal),
            Id = meal.Id,
            Servings = meal.Servings,
            MealIngredients = [.. meal.MealIngredients.Select(mI => new MealIngredientResponseDto
            {
                QuantityUsed = mI.QuantityUsed,
                Ingredient = mI.Ingredient.ToResponseDto()
            })],
            Nutrients = CalculateNutrients(meal),
            Name = meal.Name
        };
    }

    private static NutrientResponseDto CalculateNutrients(Meal meal)
    {
        static decimal? AddNullable(decimal? nutrient, decimal ratio, decimal? total) => nutrient.HasValue ? (total ?? 0) + (nutrient.Value * ratio) : total;

        var nutrients = new NutrientResponseDto();

        foreach (var mI in meal.MealIngredients)
        {
            var ingredient = mI.Ingredient;

            decimal ratio = mI.QuantityUsed / 100m;

            nutrients.Carbohydrate += ingredient.Macro.Carbohydrate * ratio;
            nutrients.Energy += ingredient.Energy * ratio;
            nutrients.Fat += ingredient.Macro.Fat * ratio;
            nutrients.Fiber += ingredient.Macro.Fiber * ratio;
            nutrients.Protein += ingredient.Macro.Protein * ratio;
            nutrients.SaturatedFat += ingredient.Macro.SaturatedFat * ratio;
            nutrients.Sugar += ingredient.Macro.Sugar * ratio;

            nutrients.AlphaLinolenicAcid = AddNullable(ingredient.Macro.AlphaLinolenicAcid, ratio, nutrients.AlphaLinolenicAcid);
            nutrients.LinoleicAcid = AddNullable(ingredient.Macro.LinoleicAcid, ratio, nutrients.LinoleicAcid);

            nutrients.Chloride += ingredient.Mineral.Chloride * ratio;
            nutrients.Sodium += ingredient.Mineral.Sodium * ratio;

            nutrients.Calcium = AddNullable(ingredient.Mineral.Calcium, ratio, nutrients.Calcium);
            nutrients.Chromium = AddNullable(ingredient.Mineral.Chromium, ratio, nutrients.Chromium);
            nutrients.Copper = AddNullable(ingredient.Mineral.Copper, ratio, nutrients.Copper);
            nutrients.Fluoride = AddNullable(ingredient.Mineral.Fluoride, ratio, nutrients.Fluoride);
            nutrients.Iodine = AddNullable(ingredient.Mineral.Iodine, ratio, nutrients.Iodine);
            nutrients.Iron = AddNullable(ingredient.Mineral.Iron, ratio, nutrients.Iron);
            nutrients.Magnesium = AddNullable(ingredient.Mineral.Magnesium, ratio, nutrients.Magnesium);
            nutrients.Manganese = AddNullable(ingredient.Mineral.Manganese, ratio, nutrients.Manganese);
            nutrients.Molybdenum = AddNullable(ingredient.Mineral.Molybdenum, ratio, nutrients.Molybdenum);
            nutrients.Phosphorus = AddNullable(ingredient.Mineral.Phosphorus, ratio, nutrients.Phosphorus);
            nutrients.Potassium = AddNullable(ingredient.Mineral.Potassium, ratio, nutrients.Potassium);
            nutrients.Selenium = AddNullable(ingredient.Mineral.Selenium, ratio, nutrients.Selenium);
            nutrients.Zinc = AddNullable(ingredient.Mineral.Zinc, ratio, nutrients.Zinc);

            nutrients.Biotin = AddNullable(ingredient.Vitamin.Biotin, ratio, nutrients.Biotin);
            nutrients.Choline = AddNullable(ingredient.Vitamin.Choline, ratio, nutrients.Choline);
            nutrients.Folate = AddNullable(ingredient.Vitamin.Folate, ratio, nutrients.Folate);
            nutrients.Niacin = AddNullable(ingredient.Vitamin.Niacin, ratio, nutrients.Niacin);
            nutrients.PantothenicAcid = AddNullable(ingredient.Vitamin.PantothenicAcid, ratio, nutrients.PantothenicAcid);
            nutrients.Riboflavin = AddNullable(ingredient.Vitamin.Riboflavin, ratio, nutrients.Riboflavin);
            nutrients.Thiamin = AddNullable(ingredient.Vitamin.Thiamin, ratio, nutrients.Thiamin);
            nutrients.VitaminA = AddNullable(ingredient.Vitamin.VitaminA, ratio, nutrients.VitaminA);
            nutrients.VitaminB12 = AddNullable(ingredient.Vitamin.VitaminB12, ratio, nutrients.VitaminB12);
            nutrients.VitaminB6 = AddNullable(ingredient.Vitamin.VitaminB6, ratio, nutrients.VitaminB6);
            nutrients.VitaminC = AddNullable(ingredient.Vitamin.VitaminC, ratio, nutrients.VitaminC);
            nutrients.VitaminD = AddNullable(ingredient.Vitamin.VitaminD, ratio, nutrients.VitaminD);
            nutrients.VitaminE = AddNullable(ingredient.Vitamin.VitaminE, ratio, nutrients.VitaminE);
            nutrients.VitaminK = AddNullable(ingredient.Vitamin.VitaminK, ratio, nutrients.VitaminK);
        }

        decimal s = meal.Servings;

        static decimal? Round(decimal? value, int decimals) => value.HasValue ? Math.Round(value.Value, decimals) : null;

        return new NutrientResponseDto
        {
            Carbohydrate = Math.Round(nutrients.Carbohydrate / s, 2),
            Energy = Math.Round(nutrients.Energy / s, 2),
            Fat = Math.Round(nutrients.Fat / s, 2),
            Fiber = Math.Round(nutrients.Fiber / s, 2),
            Protein = Math.Round(nutrients.Protein / s, 2),
            SaturatedFat = Math.Round(nutrients.SaturatedFat / s, 2),
            Sugar = Math.Round(nutrients.Sugar / s, 2),

            AlphaLinolenicAcid = Round(nutrients.AlphaLinolenicAcid / s, 4),
            LinoleicAcid = Round(nutrients.LinoleicAcid / s, 4),

            Chloride = Math.Round(nutrients.Chloride / s, 4),
            Sodium = Math.Round(nutrients.Sodium / s, 4),

            Calcium = Round(nutrients.Calcium / s, 4),
            Chromium = Round(nutrients.Chromium / s, 4),
            Copper = Round(nutrients.Copper / s, 4),
            Fluoride = Round(nutrients.Fluoride / s, 4),
            Iodine = Round(nutrients.Iodine / s, 4),
            Iron = Round(nutrients.Iron / s, 4),
            Magnesium = Round(nutrients.Magnesium / s, 4),
            Manganese = Round(nutrients.Manganese / s, 4),
            Molybdenum = Round(nutrients.Molybdenum / s, 4),
            Phosphorus = Round(nutrients.Phosphorus / s, 4),
            Potassium = Round(nutrients.Potassium / s, 4),
            Selenium = Round(nutrients.Selenium / s, 4),
            Zinc = Round(nutrients.Zinc / s, 4),

            Biotin = Round(nutrients.Biotin / s, 4),
            Choline = Round(nutrients.Choline / s, 4),
            Folate = Round(nutrients.Folate / s, 4),
            Niacin = Round(nutrients.Niacin / s, 4),
            PantothenicAcid = Round(nutrients.PantothenicAcid / s, 4),
            Riboflavin = Round(nutrients.Riboflavin / s, 4),
            Thiamin = Round(nutrients.Thiamin / s, 4),
            VitaminA = Round(nutrients.VitaminA / s, 4),
            VitaminB12 = Round(nutrients.VitaminB12 / s, 4),
            VitaminB6 = Round(nutrients.VitaminB6 / s, 4),
            VitaminC = Round(nutrients.VitaminC / s, 4),
            VitaminD = Round(nutrients.VitaminD / s, 4),
            VitaminE = Round(nutrients.VitaminE / s, 4),
            VitaminK = Round(nutrients.VitaminK / s, 4)
        };
    }
}
