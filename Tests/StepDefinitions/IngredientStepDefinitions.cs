using Backend.Constants;
using Backend.DTOs;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Tests.Support;

namespace Tests.StepDefinitions;

[Binding]
public sealed class IngredientStepDefinitions(FakeUsdaService usdaService, TestDatabase database)
{
    [Given("USDA has")]
    public void GivenUsdaHas(DataTable table)
    {
        usdaService.Response = new UsdaResponseDto
        {
            FoodNutrients = [.. table.Rows.Select(row => new UsdaFoodNutrientDto
            {
                Amount = decimal.Parse(row["Amount"], CultureInfo.InvariantCulture),
                Nutrient = new UsdaNutrientDto { Id = GetUsdaId(row["Nutrient"]) }
            })]
        };
    }

    [When("I add an ingredient with a USDA id")]
    public async Task WhenIAddAnIngredientWithAUsdaId()
    {
        await AddIngredientAsync(0, 1);
    }

    [When("I add an ingredient without a USDA id")]
    public async Task WhenIAddAnIngredientWithoutAUsdaId()
    {
        await AddIngredientAsync(0, null);
    }

    [When("I add an ingredient with {decimal} g of salt")]
    public async Task WhenIAddAnIngredientWithSalt(decimal salt)
    {
        await AddIngredientAsync(salt, null);
    }

    [Then("the ingredient has")]
    public async Task ThenTheIngredientHas(DataTable table)
    {
        database.Context.ChangeTracker.Clear();

        var ingredient = await database.Context.Ingredients
            .Include(i => i.Macro)
            .Include(i => i.Mineral)
            .Include(i => i.Vitamin)
            .SingleAsync();

        object[] parts = [ingredient.Macro, ingredient.Mineral, ingredient.Vitamin];

        foreach (var row in table.Rows)
        {
            var part = parts.Single(p => p.GetType().GetProperty(row["Nutrient"]) != null);

            decimal? actual = (decimal?)part.GetType().GetProperty(row["Nutrient"])!.GetValue(part);
            decimal? expected = row["Amount"] == "null" ? null : decimal.Parse(row["Amount"], CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }
    }

    private async Task AddIngredientAsync(decimal salt, int? usdaId)
    {
        var ingredientService = new IngredientService(database.Context, usdaService);

        await ingredientService.AddAsync(new IngredientCreateDto
        {
            Salt = salt,
            UsdaId = usdaId,
            Name = "Test ingredient"
        });
    }

    private static int GetUsdaId(string nutrient)
    {
        var field = typeof(Nutrients).GetField(nutrient)!;

        return ((Nutrient)field.GetValue(null)!).UsdaId!.Value;
    }
}
