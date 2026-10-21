using Backend.Constants;
using Backend.Data;
using Backend.DTOs;
using Backend.Mappers;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class IngredientService(AppDbContext context, IUsdaService usdaService) : IIngredientService
{
    public async Task<IngredientResponseDto> AddAsync(IngredientCreateDto dto)
    {
        decimal chlorideFactor = 0.6066m;
        decimal sodiumFactor = 0.3934m;
        decimal? vitaminK = null;
        UsdaResponseDto? data = null;

        if (dto.UsdaId.HasValue)
        {
            data = await usdaService.GetByIdAsync(dto.UsdaId.Value);

            decimal? k1 = FindNutrient(Nutrients.VitaminK1);
            decimal? k1D = FindNutrient(Nutrients.VitaminK1Dihydro);
            decimal? k2 = FindNutrient(Nutrients.VitaminK2);

            if (k1 != null || k1D != null || k2 != null)
            {
                vitaminK = (k1 ?? 0) + (k1D ?? 0) + (k2 ?? 0);
            }
        }

        decimal? FindNutrient(Nutrient n) => data?.FoodNutrients.FirstOrDefault(x => x.Nutrient.Id == n.UsdaId)?.Amount;

        var ingredient = new Ingredient
        {
            Energy = dto.Energy,
            Name = dto.Name,
            Price = dto.Price,
            Quantity = dto.Quantity,
            UsdaId = dto.UsdaId,
            Macro = new Macro
            {
                Carbohydrate = dto.Carbohydrate,
                Fat = dto.Fat,
                Fiber = dto.Fiber,
                Protein = dto.Protein,
                SaturatedFat = dto.SaturatedFat,
                Sugar = dto.Sugar,
                AlphaLinolenicAcid = FindNutrient(Nutrients.AlphaLinolenicAcid) ?? FindNutrient(Nutrients.Pufa183),
                LinoleicAcid = FindNutrient(Nutrients.LinoleicAcid) ?? FindNutrient(Nutrients.Pufa182)
            },
            Mineral = new Mineral
            {
                Chloride = dto.Salt * chlorideFactor * 1000,
                Sodium = dto.Salt * sodiumFactor * 1000,
                Calcium = FindNutrient(Nutrients.Calcium),
                Chromium = null,
                Copper = FindNutrient(Nutrients.Copper),
                Fluoride = FindNutrient(Nutrients.Fluoride),
                Iodine = null,
                Iron = FindNutrient(Nutrients.Iron),
                Magnesium = FindNutrient(Nutrients.Magnesium),
                Manganese = FindNutrient(Nutrients.Manganese),
                Molybdenum = null,
                Phosphorus = FindNutrient(Nutrients.Phosphorus),
                Potassium = FindNutrient(Nutrients.Potassium),
                Selenium = FindNutrient(Nutrients.Selenium),
                Zinc = FindNutrient(Nutrients.Zinc)
            },
            Vitamin = new Vitamin
            {
                Biotin = null,
                Choline = FindNutrient(Nutrients.Choline),
                Folate = FindNutrient(Nutrients.Folate),
                Niacin = FindNutrient(Nutrients.Niacin),
                PantothenicAcid = FindNutrient(Nutrients.PantothenicAcid),
                Riboflavin = FindNutrient(Nutrients.Riboflavin),
                Thiamin = FindNutrient(Nutrients.Thiamin),
                VitaminA = FindNutrient(Nutrients.VitaminA),
                VitaminB12 = FindNutrient(Nutrients.VitaminB12),
                VitaminB6 = FindNutrient(Nutrients.VitaminB6),
                VitaminC = FindNutrient(Nutrients.VitaminC),
                VitaminD = FindNutrient(Nutrients.VitaminD),
                VitaminE = FindNutrient(Nutrients.VitaminE),
                VitaminK = vitaminK
            }
        };

        context.Ingredients.Add(ingredient);
        await context.SaveChangesAsync();

        return ingredient.ToResponseDto();
    }

    public async Task<IngredientResponseDto?> GetByIdAsync(int id)
    {
        var ingredient = await context.Ingredients.FirstOrDefaultAsync(i => i.Id == id);

        if (ingredient == null)
        {
            return null;
        }

        return ingredient.ToResponseDto();
    }

    public async Task<List<IngredientResponseDto>> GetAllAsync()
    {
        var ingredients = await context.Ingredients.ToListAsync();

        return [.. ingredients.Select(i => i.ToResponseDto())];
    }
}
