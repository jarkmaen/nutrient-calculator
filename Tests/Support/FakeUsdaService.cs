using Backend.DTOs;
using Backend.Services;

namespace Tests.Support;

public sealed class FakeUsdaService : IUsdaService
{
    public UsdaResponseDto? Response { get; set; }

    public Task<UsdaResponseDto?> GetByIdAsync(int id)
    {
        return Task.FromResult(Response);
    }
}
