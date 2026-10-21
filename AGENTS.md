# AGENTS.md

This file provides guidance to AI coding agents when working with code in this repository.

## Overview

Nutrient Calculator is a single-page app: select meals and see their combined macros, fatty acids, minerals and vitamins per serving against DRI targets. Selection is UI state only; nothing about it is stored.

- `NutrientCalculator.slnx` — .NET 10 solution with `Backend`, `CLI`, `Tests`.
- `Backend/` — ASP.NET Core Web API, EF Core code first, SQL Server.
- `Frontend/` — React 19, TypeScript, Vite, Tailwind v4, React Compiler. Not part of the solution.
- `CLI/` — console app for adding ingredients and meals over HTTP.
- `Tests/` — Reqnroll (Gherkin) on xUnit, referencing `Backend`.
- `.agents/skills/` — `add-ingredient` (from a pasted product page) and `add-meal`.

## Commands

From the repo root:
```
dotnet build NutrientCalculator.slnx
dotnet run --project Backend --launch-profile https   # https://localhost:7203 + http://localhost:5020; Scalar at /scalar
dotnet run --project CLI                              # needs the backend running
dotnet test Tests
```

Migrations are run from Visual Studio's Package Manager Console:
```
Add-Migration <Name>
Update-Database
Script-DbContext -Output "$(Get-Location)\Backend\schema.sql"
```
`Backend/schema.sql` is generated; regenerate it after each migration.

From `Frontend/`:
```
npm run dev              # proxies /api to http://localhost:5020
npm run build
npm run lint
npm run generate-types   # src/types/api.ts from Backend/obj/Backend.json
```
`npm install` needs `--legacy-peer-deps` (`openapi-typescript` peer-depends on TypeScript 5; the project uses 6).

## Configuration

Backend user secrets:
- `ConnectionStrings:DefaultConnection` — SQL Server Express `localhost\SQLEXPRESS`, database `NutrientCalculator`
- `UsdaApiKey` — USDA FoodData Central API key

Query the database with `sqlcmd -S localhost\SQLEXPRESS -d NutrientCalculator -E -C`.

## Architecture

### Backend

Controller → Service (interface + implementation, scoped) → `AppDbContext`. Routes: `/api/dri`, `/api/ingredients`, `/api/meals`.

- **Model:** `Ingredient` has one `Macro`, `Mineral` and `Vitamin` row each. `Meal` links to ingredients through `MealIngredient` with `QuantityUsed` (g/ml). Decimals are `(19, 8)`.
- **Values are per 100 g/ml.** `Ingredient.Quantity` and `Price` are the package size and price.
- **`Constants/Nutrients.cs`** is the nutrient registry: name, unit, DRI (`Recommended`, `UpperLimit`) and USDA nutrient id. `DriService` returns every field with a DRI via reflection, so adding one changes `/api/dri` and the count in `Tests/Features/Dri.feature`. Entries without a DRI (`Pufa182`, `Pufa183`, `VitaminK1`, `VitaminK1Dihydro`, `VitaminK2`) exist only for USDA lookup.
- **`IngredientService.AddAsync`:** macros, energy and salt come from the request. With a `UsdaId`, micronutrients and fatty acids are fetched from FoodData Central and matched by USDA nutrient id. ALA and LA fall back to PUFA 18:3 and 18:2; vitamin K is the sum of K1, K1 dihydro and K2. Salt is split into sodium and chloride. `UsdaService` returns `null` on any error.
- **`MealService`:** scales each ingredient by `QuantityUsed / 100`, sums, and divides by `Servings`. Macros round to 2 dp, others to 4 dp. Nullable nutrients stay `null` when no ingredient has a value. Price per serving is `Price * QuantityUsed / Quantity`.
- **New nutrient:** `Nutrients.cs`, the `Macro`/`Mineral`/`Vitamin` model + migration, `NutrientResponseDto`, `IngredientService.AddAsync`, `MealService.CalculateNutrients`, and `Frontend/src/constants/nutrients.ts`.

### Frontend

- `App.tsx` fetches `/api/dri` and `/api/meals` once into `AppContext` (`useAppContext()`), which also holds `selectedMealIds`.
- `utils/nutrients.ts` sums the selected meals and sets each nutrient's state (`low`, `met`, `ok`, `over`, `none`).
- `constants/nutrients.ts` sets the groups and order on screen.
- `types/api.ts` is generated; `types/index.ts` exports aliases (`Dri`, `Meal`, `Nutrients`). After changing a DTO, build the backend, then run `npm run generate-types`.
- Desktop layout is a sidebar plus nutrient columns; below `md` the meal list is a bottom sheet.

### CLI

A separate HTTP client with its own DTOs, not a reference to `Backend`. `Program.cs` reads `ApiBaseUrl` from `CLI/appsettings.json` and passes one `HttpClient` to `IngredientService` and `MealService`, which `TextUI` uses. `TextUI` validates input before sending.

## Style

- CRLF line endings.
- Alphabetical order for imports, usings, object properties, JSX props, JSON keys, CSS properties and Tailwind classes (`md:` classes sort by full string).
- Models: `Id`, foreign keys, required properties A–Z, nullable properties A–Z, navigation properties.
- DTOs: by type, then name.
- C#: `var` for objects and explicit types for primitives, primary constructors for DI, file-scoped namespaces, collection expressions.
- Prettier: 4-space indent, no trailing commas.
- Commits: conventional (`feat`, `fix`, `refactor`, `chore`, `docs`, `style`), short, no commas.
