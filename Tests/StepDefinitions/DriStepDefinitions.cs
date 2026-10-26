using Backend.DTOs;
using Backend.Services;
using System.Globalization;
using System.Text.Json;

namespace Tests.StepDefinitions;

[Binding]
public sealed class DriStepDefinitions
{
    private List<DriResponseDto>? result;
    private readonly DriService driService = new();

    [When("I request the DRI data")]
    public void WhenIRequestTheDRIData()
    {
        result = driService.GetAll();
    }

    [Then("every meal nutrient has a DRI")]
    public void ThenEveryMealNutrientHasADri()
    {
        var expected = typeof(NutrientResponseDto).GetProperties().Select(p => JsonNamingPolicy.CamelCase.ConvertName(p.Name)).Order();

        Assert.Equal(expected, result!.Select(d => d.Key).Order());
    }

    [Then("the DRI data does not include")]
    public void ThenTheDriDataDoesNotInclude(DataTable table)
    {
        foreach (var row in table.Rows)
        {
            Assert.DoesNotContain(result!, d => d.Key == row["Key"]);
        }
    }

    [Then("the DRI data includes")]
    public void ThenTheDriDataIncludes(DataTable table)
    {
        foreach (var row in table.Rows)
        {
            var dri = result!.Single(d => d.Key == row["Key"]);

            Assert.Equal(row["Name"], dri.Name);
            Assert.Equal(ParseNullable(row["Recommended"]), dri.Recommended);
            Assert.Equal(row["Unit"], dri.Unit);
            Assert.Equal(ParseNullable(row["UpperLimit"]), dri.UpperLimit);
        }
    }

    private static decimal? ParseNullable(string value)
    {
        return value == "null" ? null : decimal.Parse(value, CultureInfo.InvariantCulture);
    }
}
