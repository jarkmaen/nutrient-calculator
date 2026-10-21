using Backend.DTOs;

namespace Backend.Services;

public interface IMealService
{
    Task<List<MealResponseDto>> GetAllAsync();
    Task<MealResponseDto?> AddAsync(MealCreateDto dto);
    Task<MealResponseDto?> GetByIdAsync(int id);
}
