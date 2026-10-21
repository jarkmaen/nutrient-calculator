using Backend.DTOs;

namespace Backend.Services;

public interface IIngredientService
{
    Task<IngredientResponseDto> AddAsync(IngredientCreateDto dto);
    Task<IngredientResponseDto?> GetByIdAsync(int id);
    Task<List<IngredientResponseDto>> GetAllAsync();
}
