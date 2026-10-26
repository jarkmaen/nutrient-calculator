using Backend.DTOs;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Tests.Support;

namespace Tests.StepDefinitions;

[Binding]
public sealed class MealStepDefinitions(FakeUsdaService usdaService, TestDatabase database)
{
    private MealResponseDto? meal;

    [Given("the ingredients")]
    public async Task GivenTheIngredients(DataTable table)
    {
        var ingredientService = new IngredientService(database.Context, usdaService);

        foreach (var dto in table.CreateSet(() => new IngredientCreateDto { Quantity = 100 }))
        {
            await ingredientService.AddAsync(dto);
        }
    }

    [When("I add a meal of {int} serving(s) using this/these quantity/quantities")]
    public async Task WhenIAddAMealOfServings(int servings, DataTable table)
    {
        var mealService = new MealService(database.Context);

        List<MealIngredientCreateDto> mealIngredients = [];

        foreach (var row in table.Rows)
        {
            var ingredient = await database.Context.Ingredients.SingleOrDefaultAsync(i => i.Name == row["Ingredient"]);

            mealIngredients.Add(new MealIngredientCreateDto
            {
                QuantityUsed = decimal.Parse(row["QuantityUsed"], CultureInfo.InvariantCulture),
                IngredientId = ingredient?.Id ?? int.MaxValue
            });
        }

        meal = await mealService.AddAsync(new MealCreateDto
        {
            Servings = servings,
            MealIngredients = mealIngredients,
            Name = "Test meal"
        });
    }

    [Then("the meal costs {decimal} per serving")]
    public void ThenTheMealCostsPerServing(decimal price)
    {
        Assert.Equal(price, meal!.Price);
    }

    [Then("the meal has")]
    public void ThenTheMealHas(DataTable table)
    {
        foreach (var row in table.Rows)
        {
            decimal? actual = (decimal?)typeof(NutrientResponseDto).GetProperty(row["Nutrient"])!.GetValue(meal!.Nutrients);
            decimal? expected = row["Amount"] == "null" ? null : decimal.Parse(row["Amount"], CultureInfo.InvariantCulture);

            Assert.Equal(expected, actual);
        }
    }

    [Then("the meal is rejected")]
    public async Task ThenTheMealIsRejected()
    {
        Assert.Null(meal);
        Assert.False(await database.Context.Meals.AnyAsync());
    }
}
