using Backend.DTOs;
using Backend.Models;

namespace Backend.Mappers;

public static class IngredientMapper
{
    public static IngredientResponseDto ToResponseDto(this Ingredient ingredient)
    {
        return new IngredientResponseDto
        {
            Price = ingredient.Price,
            Quantity = ingredient.Quantity,
            Id = ingredient.Id,
            UsdaId = ingredient.UsdaId,
            Name = ingredient.Name
        };
    }
}
