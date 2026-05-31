using HedgePair.API.Data;
using HedgePair.API.Interfaces;
using HedgePair.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HedgePair.API.Repositories;

public class FinancialInstrumentRepository : IFinancialInstrumentRepository
{
    private readonly AppDbContext _ctx;
    public FinancialInstrumentRepository(AppDbContext ctx) => _ctx = ctx;

    /// <inheritdoc/>
    public async Task<IEnumerable<FinancialInstrument>> GetActiveUnpairedAsync()
    {
        // ✅ Get all paired IDs using SQL-compatible operations
        var pairedIds = await _ctx.HedgePairs
            .Select(hp => hp.BondFinId)
            .Union(_ctx.HedgePairs.Select(hp => hp.SwapFinId))
            .ToListAsync();

        // ✅ Get only active instruments that are not paired
        return await _ctx.FinancialInstruments
            .Where(fi => fi.DealStatus == "Active" && !pairedIds.Contains(fi.FinId))
            .OrderBy(fi => fi.DealType)
            .ThenBy(fi => fi.DealNumber)
            .ToListAsync();

    }

    /// <inheritdoc/>
    public async Task<FinancialInstrument?> GetByIdAsync(int finId)
        => await _ctx.FinancialInstruments.FindAsync(finId);

    /// <inheritdoc/>
    public async Task<bool> IsAlreadyPairedAsync(int finId)
        => await _ctx.HedgePairs
            .AnyAsync(hp => hp.BondFinId == finId || hp.SwapFinId == finId);
}
