using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealsController(IMealService mealService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<MealResponseDto>>> GetAll()
    {
        return await mealService.GetAllAsync();
    }

    [HttpPost]
    public async Task<ActionResult<MealResponseDto>> Create(MealCreateDto dto)
    {
        var createdMeal = await mealService.AddAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = createdMeal.Id }, createdMeal);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MealResponseDto>> GetById(int id)
    {
        var meal = await mealService.GetByIdAsync(id);

        if (meal == null)
        {
            return NotFound();
        }

        return meal;
    }
}
