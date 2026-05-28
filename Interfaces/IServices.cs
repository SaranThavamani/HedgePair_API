using HedgePair.API.DTOs;

namespace HedgePair.API.Interfaces;

public interface IFinancialInstrumentService
{
    Task<IEnumerable<FinancialInstrumentDto>> GetActiveUnpairedAsync();
}

public interface IHedgePairService
{
    Task<IEnumerable<HedgePairDto>> GetAllAsync();
    Task<(HedgePairDto? Result, string? Error, int StatusCode)> CreateAsync(CreatePairDto dto);
    Task<(bool Success, string? Error)> DeleteAsync(int hedgePairId);
}
