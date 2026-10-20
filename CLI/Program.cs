using CLI.Services;
using CLI.UI;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var apiBaseUrl = configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("ApiBaseUrl is missing from appsettings.json");

var httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };
var ingredientService = new IngredientService(httpClient);
var mealService = new MealService(httpClient);
var textUI = new TextUI(ingredientService, mealService);

await textUI.RunAsync();
