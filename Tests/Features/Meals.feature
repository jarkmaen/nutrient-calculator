Feature: Meals

Meal nutrients and price are calculated per serving

@meals
Scenario: Ingredients are scaled and summed
	Given the ingredients
		| Name | Fat | Protein |
		| Milk | 1.5 | 3.5     |
		| Oats | 7   | 13      |
	When I add a meal of 1 serving using these quantities
		| Ingredient | QuantityUsed |
		| Milk       | 200          |
		| Oats       | 50           |
	Then the meal has
		| Nutrient | Amount |
		| Fat      | 6.5    |
		| Protein  | 13.5   |

@meals
Scenario: Nutrients are divided by servings
	Given the ingredients
		| Name | Carbohydrate |
		| Rice | 80           |
	When I add a meal of 3 servings using this quantity
		| Ingredient | QuantityUsed |
		| Rice       | 300          |
	Then the meal has
		| Nutrient     | Amount |
		| Carbohydrate | 80     |

@meals
Scenario: Macros round to 2 decimals and other nutrients to 4
	Given the ingredients
		| Name     | Protein | Salt |
		| Crackers | 2       | 1    |
	When I add a meal of 3 servings using this quantity
		| Ingredient | QuantityUsed |
		| Crackers   | 1            |
	Then the meal has
		| Nutrient | Amount |
		| Protein  | 0.01   |
		| Sodium   | 1.3113 |

@meals
Scenario: Price is per serving
	Given the ingredients
		| Name | Price | Quantity |
		| Oats | 2     | 1000     |
	When I add a meal of 2 servings using this quantity
		| Ingredient | QuantityUsed |
		| Oats       | 500          |
	Then the meal costs 0.5 per serving

@meals
Scenario: Nutrients stay null when no ingredient has a value
	Given the ingredients
		| Name |
		| Milk |
		| Oats |
	When I add a meal of 1 serving using these quantities
		| Ingredient | QuantityUsed |
		| Milk       | 200          |
		| Oats       | 50           |
	Then the meal has
		| Nutrient | Amount |
		| Calcium  | null   |

@meals
Scenario: Nutrients are summed from the ingredients that have a value
	Given USDA has
		| Nutrient | Amount |
		| Calcium  | 120    |
	And the ingredients
		| Name | UsdaId |
		| Milk | 1      |
		| Oats |        |
	When I add a meal of 1 serving using these quantities
		| Ingredient | QuantityUsed |
		| Milk       | 200          |
		| Oats       | 50           |
	Then the meal has
		| Nutrient | Amount |
		| Calcium  | 240    |
