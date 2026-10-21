import type { components } from "./api";

export interface MealContribution {
    id: number;
    name: string;
    value: number;
}

export interface NutrientTotal {
    contributions: MealContribution[];
    dri: Dri;
    state: NutrientState;
    value: number;
}

export type Dri = components["schemas"]["DriResponseDto"];
export type Meal = components["schemas"]["MealResponseDto"];
export type Nutrients = components["schemas"]["NutrientResponseDto"];

export type NutrientState = "low" | "met" | "none" | "ok" | "over";
