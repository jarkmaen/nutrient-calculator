---
name: add-meal
description: Add a meal to the database from ingredients already in it. Use when the user wants to add a meal.
---

1. Get the meal name, servings, and each ingredient with the grams used.
2. Look up the ingredient ids with `sqlcmd -S localhost\SQLEXPRESS -d NutrientCalculator -E -C`. Ask the user when a name matches several ingredients or none.
3. Show the user the meal: name, servings, each ingredient with its id and grams, and the per-serving energy, protein and price calculated from the ingredients. Save after they approve.
4. POST to `https://localhost:7203/api/meals` with `name`, `servings` and `mealIngredients` (`ingredientId`, `quantityUsed`).
5. Confirm the saved meal from `GET https://localhost:7203/api/meals`.
