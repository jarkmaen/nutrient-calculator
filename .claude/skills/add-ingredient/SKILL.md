---
name: add-ingredient
description: Add an ingredient to the database from a pasted grocery store product page. Use when the user pastes a product page.
---

1. From the pasted page, take the `POST /api/ingredients` fields: `name` (short, in English), `quantity` (package size in g or ml), `price` (€), and per 100 g/ml `energy` (kcal), `fat`, `saturatedFat`, `carbohydrate`, `sugar`, `protein`, `salt`, `fiber`.
2. For a single unprocessed ingredient, find its match in USDA FoodData Central SR Legacy and use it as `usdaId`. Processed products get no `usdaId`. The API key is `UsdaApiKey` in `dotnet user-secrets list --project Backend`; load it into a shell variable without printing it.
3. Without a `usdaId`, also take any extra vitamins and minerals the label lists.
4. Show the user all values, the USDA id with its SR Legacy description, and any extra nutrients. Save after they approve.
5. POST to `https://localhost:7203/api/ingredients`. Write extra nutrients to the new row in `Minerals` / `Vitamins` with `sqlcmd -S localhost\SQLEXPRESS -d NutrientCalculator -E -C`, in the units from `Backend/Constants/Nutrients.cs`.
6. Confirm the saved values from the database.
