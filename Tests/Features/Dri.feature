Feature: DRI

DRI (Dietary Reference Intake) targets for every meal nutrient

@dri
Scenario: Every meal nutrient has a DRI
	When I request the DRI data
	Then every meal nutrient has a DRI

@dri
Scenario: DRI values come from the nutrient registry
	When I request the DRI data
	Then the DRI data includes
		| Key      | Name      | Recommended | Unit      | UpperLimit |
		| sodium   | Sodium    | 1500        | Milligram | 2300       |
		| sugar    | Sugar     | null        | Gram      | 64         |
		| vitaminK | Vitamin K | 120         | Microgram | null       |

@dri
Scenario: Nutrients used only for USDA lookup are left out
	When I request the DRI data
	Then the DRI data does not include
		| Key              |
		| pufa182          |
		| pufa183          |
		| vitaminK1        |
		| vitaminK1Dihydro |
		| vitaminK2        |
