using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController(IIngredientService ingredientService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<IngredientResponseDto>> Create(IngredientCreateDto dto)
    {
        var createdIngredient = await ingredientService.AddAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = createdIngredient.Id }, createdIngredient);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IngredientResponseDto>> GetById(int id)
    {
        var ingredient = await ingredientService.GetByIdAsync(id);

        if (ingredient == null)
        {
            return NotFound();
        }

        return ingredient;
    }

    [HttpGet]
    public async Task<ActionResult<List<IngredientResponseDto>>> GetAll()
    {
        return await ingredientService.GetAllAsync();
    }
}
