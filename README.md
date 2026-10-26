# Nutrient Calculator

## Overview

A full stack application for calculating the nutrients of meals. Selecting meals shows their combined macronutrients, vitamins and minerals. Each nutrient is compared against its Dietary Reference Intake (DRI), showing whether it falls short, meets the recommended amount or goes over the upper limit.

```mermaid
graph LR
    CLI[.NET<br/>CLI]
    Frontend[React<br/>Frontend]
    USDA{{"USDA FoodData<br/>Central API"}}
    User((User))

    subgraph Backend [ASP.NET Core Web API]
        Controllers[Controllers]
        Services[Services]
        DbContext[EF Core<br/>DbContext]
    end

    DB[(SQL Server<br/>Database)]

    User <--> Frontend
    User <--> CLI

    Frontend <-- REST API --> Controllers
    CLI <-- REST API --> Controllers
    Controllers <--> Services
    Services <--> DbContext
    DbContext <--> DB
    Services <-- HTTPS --> USDA
```

## Tech stack

- **Frontend:** React, Tailwind
- **Backend:** ASP.NET Core, Entity Framework Core, REST API
- **Database:** SQL Server
- **Other:** C#, Reqnroll, TypeScript

## Screenshots

| Main view                                        |
| ------------------------------------------------ |
| ![Main view](Documentation/Images/main_view.png) |
