using CLI.DTOs;
using System.Net.Http.Json;

namespace CLI.Services;

public class MealService(HttpClient httpClient)
{
    public async Task AddAsync(MealCreateDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("api/meals", dto);

        response.EnsureSuccessStatusCode();
    }
}
