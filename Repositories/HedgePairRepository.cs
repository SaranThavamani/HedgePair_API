using HedgePair.API.Data;
using HedgePair.API.Interfaces;
using HedgePair.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HedgePair.API.Repositories;

public class HedgePairRepository : IHedgePairRepository
{
    private readonly AppDbContext _ctx;
    public HedgePairRepository(AppDbContext ctx) => _ctx = ctx;

    /// <inheritdoc/>
    public async Task<IEnumerable<Models.HedgePair>> GetAllAsync()
        => await _ctx.HedgePairs
            .Include(hp => hp.Bond)
            .Include(hp => hp.Swap)
            .OrderByDescending(hp => hp.HedgePairId)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<Models.HedgePair?> GetByIdAsync(int hedgePairId)
        => await _ctx.HedgePairs
            .Include(hp => hp.Bond)
            .Include(hp => hp.Swap)
            .FirstOrDefaultAsync(hp => hp.HedgePairId == hedgePairId);

    /// <inheritdoc/>
    public async Task<Models.HedgePair> CreateAsync(Models.HedgePair hedgePair)
    {
        _ctx.HedgePairs.Add(hedgePair);
        await _ctx.SaveChangesAsync();
        // Reload navigation properties
        await _ctx.Entry(hedgePair).Reference(hp => hp.Bond).LoadAsync();
        await _ctx.Entry(hedgePair).Reference(hp => hp.Swap).LoadAsync();
        return hedgePair;
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Models.HedgePair hedgePair)
    {
        _ctx.HedgePairs.Remove(hedgePair);
        await _ctx.SaveChangesAsync();
    }
}
