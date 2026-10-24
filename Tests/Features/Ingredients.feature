Feature: Ingredients

Ingredients get macros from their label and micronutrients from USDA FoodData Central

@ingredients
Scenario: Salt is split into sodium and chloride
	When I add an ingredient with 1 g of salt
	Then the ingredient has
		| Nutrient | Amount |
		| Chloride | 606.6  |
		| Sodium   | 393.4  |

@ingredients
Scenario: Micronutrients come from USDA
	Given USDA has
		| Nutrient | Amount |
		| Calcium  | 120    |
		| VitaminC | 30     |
	When I add an ingredient with a USDA id
	Then the ingredient has
		| Nutrient | Amount |
		| Calcium  | 120    |
		| Iron     | null   |
		| VitaminC | 30     |

@ingredients
Scenario: Fatty acids fall back to PUFA values
	Given USDA has
		| Nutrient | Amount |
		| Pufa182  | 10     |
		| Pufa183  | 2.5    |
	When I add an ingredient with a USDA id
	Then the ingredient has
		| Nutrient           | Amount |
		| AlphaLinolenicAcid | 2.5    |
		| LinoleicAcid       | 10     |

@ingredients
Scenario: Exact fatty acids are preferred over PUFA values
	Given USDA has
		| Nutrient           | Amount |
		| AlphaLinolenicAcid | 1      |
		| LinoleicAcid       | 8      |
		| Pufa182            | 10     |
		| Pufa183            | 2.5    |
	When I add an ingredient with a USDA id
	Then the ingredient has
		| Nutrient           | Amount |
		| AlphaLinolenicAcid | 1      |
		| LinoleicAcid       | 8      |

@ingredients
Scenario: Vitamin K is the sum of its forms
	Given USDA has
		| Nutrient         | Amount |
		| VitaminK1        | 10     |
		| VitaminK1Dihydro | 2      |
		| VitaminK2        | 3      |
	When I add an ingredient with a USDA id
	Then the ingredient has
		| Nutrient | Amount |
		| VitaminK | 15     |

@ingredients
Scenario: Ingredients without a USDA id have no micronutrients
	Given USDA has
		| Nutrient | Amount |
		| Calcium  | 120    |
	When I add an ingredient without a USDA id
	Then the ingredient has
		| Nutrient | Amount |
		| Calcium  | null   |
