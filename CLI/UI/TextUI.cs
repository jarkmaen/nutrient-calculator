using CLI.DTOs;
using CLI.Services;
using System.Globalization;

namespace CLI.UI;

public class TextUI(IngredientService ingredientService, MealService mealService)
{
    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("nutrient calculator");
            Console.WriteLine();
            Console.WriteLine("1. add ingredient");
            Console.WriteLine("2. list ingredients");
            Console.WriteLine("3. add meal");
            Console.WriteLine("4. exit");
            Console.WriteLine();
            Console.Write("> ");

            string? choice = Console.ReadLine()?.Trim();

            if (choice == "1")
            {
                await AddIngredientAsync();
            }
            else if (choice == "2")
            {
                await ListIngredientsAsync();
            }
            else if (choice == "3")
            {
                await AddMealAsync();
            }
            else if (choice == "4" || choice == null)
            {
                break;
            }
        }
    }

    private async Task AddIngredientAsync()
    {
        Console.Clear();
        Console.WriteLine("nutrient calculator > add ingredient");
        Console.WriteLine();

        int? usdaId = ReadOptionalInt("usda id  : ");
        string name = ReadString("name     : ");
        decimal quantity = ReadDecimal("quantity : ");
        decimal price = ReadDecimal("price    : ");

        Console.WriteLine();

        decimal energy = ReadDecimal("energy   : ");
        decimal fat = ReadDecimal("fat      : ");
        decimal saturatedFat = ReadDecimal("sat. fat : ");
        decimal carbohydrate = ReadDecimal("carbs    : ");
        decimal sugar = ReadDecimal("sugar    : ");
        decimal protein = ReadDecimal("protein  : ");
        decimal salt = ReadDecimal("salt     : ");
        decimal fiber = ReadDecimal("fiber    : ");

        var ingredient = new IngredientCreateDto
        {
            Carbohydrate = carbohydrate,
            Energy = energy,
            Fat = fat,
            Fiber = fiber,
            Price = price,
            Protein = protein,
            Quantity = quantity,
            Salt = salt,
            SaturatedFat = saturatedFat,
            Sugar = sugar,
            UsdaId = usdaId,
            Name = name
        };

        Console.WriteLine();

        try
        {
            await ingredientService.AddAsync(ingredient);

            Console.WriteLine("ingredient added.");
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("could not add ingredient. is the backend running?");
        }

        Console.WriteLine();
        Console.Write("press any key to continue...");
        Console.ReadKey();
    }

    private async Task AddMealAsync()
    {
        Console.Clear();
        Console.WriteLine("nutrient calculator > add meal");
        Console.WriteLine();

        string name = ReadString("name     : ");
        int servings = ReadInt("servings : ");

        Console.WriteLine();
        Console.WriteLine("leave id empty to finish.");

        var mealIngredients = new List<MealIngredientCreateDto>();

        while (true)
        {
            Console.WriteLine();

            int? ingredientId = ReadOptionalInt("id       : ");

            if (ingredientId == null)
            {
                break;
            }

            decimal quantityUsed = ReadDecimal("quantity : ");

            mealIngredients.Add(new MealIngredientCreateDto
            {
                QuantityUsed = quantityUsed,
                IngredientId = ingredientId.Value
            });
        }

        Console.WriteLine();

        if (mealIngredients.Count == 0)
        {
            Console.WriteLine("no ingredients given. meal not added.");
        }
        else
        {
            var meal = new MealCreateDto
            {
                Servings = servings,
                MealIngredients = mealIngredients,
                Name = name
            };

            try
            {
                await mealService.AddAsync(meal);

                Console.WriteLine("meal added.");
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("could not add meal. check the ingredient ids and that the backend is running.");
            }
        }

        Console.WriteLine();
        Console.Write("press any key to continue...");
        Console.ReadKey();
    }

    private async Task ListIngredientsAsync()
    {
        Console.Clear();
        Console.WriteLine("nutrient calculator > list ingredients");
        Console.WriteLine();

        try
        {
            var ingredients = await ingredientService.GetAllAsync();

            if (ingredients.Count == 0)
            {
                Console.WriteLine("no ingredients found.");
            }
            else
            {
                foreach (var ingredient in ingredients)
                {
                    Console.WriteLine($"{ingredient.Id,3}  {ingredient.Name}");
                }
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("could not load ingredients. is the backend running?");
        }

        Console.WriteLine();
        Console.Write("press any key to continue...");
        Console.ReadKey();
    }

    private static decimal ReadDecimal(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? input = Console.ReadLine()?.Replace(',', '.');

            if (decimal.TryParse(input, CultureInfo.InvariantCulture, out decimal value) && value >= 0)
            {
                return value;
            }

            Console.WriteLine("enter a number of 0 or more.");
        }
    }

    private static int ReadInt(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value) && value >= 1)
            {
                return value;
            }

            Console.WriteLine("enter a whole number of 1 or more.");
        }
    }

    private static int? ReadOptionalInt(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (int.TryParse(input, out int value) && value >= 1)
            {
                return value;
            }

            Console.WriteLine("enter a whole number of 1 or more, or leave it empty.");
        }
    }

    private static string ReadString(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? input = Console.ReadLine()?.Trim();

            if (!string.IsNullOrEmpty(input) && input.Length <= 255)
            {
                return input;
            }

            Console.WriteLine("enter 1 to 255 characters.");
        }
    }
}
