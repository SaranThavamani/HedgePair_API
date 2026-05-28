using HedgePair.API.DTOs;
using HedgePair.API.Interfaces;

namespace HedgePair.API.Services;

public class FinancialInstrumentService : IFinancialInstrumentService
{
    private readonly IFinancialInstrumentRepository _repo;

    public FinancialInstrumentService(IFinancialInstrumentRepository repo) => _repo = repo;

    public async Task<IEnumerable<FinancialInstrumentDto>> GetActiveUnpairedAsync()
    {
        var instruments = await _repo.GetActiveUnpairedAsync();
        return instruments.Select(fi => new FinancialInstrumentDto(
            fi.FinId,
            fi.DealNumber,
            fi.NotionalAmt,
            fi.DealType,
            fi.DealStatus
        ));
    }
}
