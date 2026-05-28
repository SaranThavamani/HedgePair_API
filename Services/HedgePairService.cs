using HedgePair.API.DTOs;
using HedgePair.API.Interfaces;
using HedgePair.API.Models;

namespace HedgePair.API.Services;

public class HedgePairService : IHedgePairService
{
    private readonly IHedgePairRepository _hpRepo;
    private readonly IFinancialInstrumentRepository _fiRepo;
    private readonly ILogger<HedgePairService> _logger;

    public HedgePairService(
        IHedgePairRepository hpRepo,
        IFinancialInstrumentRepository fiRepo,
        ILogger<HedgePairService> logger)
    {
        _hpRepo = hpRepo;
        _fiRepo = fiRepo;
        _logger = logger;
    }

    // ── Get All ──────────────────────────────────────────────────────────────

    public async Task<IEnumerable<HedgePairDto>> GetAllAsync()
    {
        var pairs = await _hpRepo.GetAllAsync();
        return pairs.Select(ToDto);
    }

    // ── Create ───────────────────────────────────────────────────────────────

    public async Task<(HedgePairDto? Result, string? Error, int StatusCode)> CreateAsync(CreatePairDto dto)
    {
        // 1. Fetch both instruments
        var bond = await _fiRepo.GetByIdAsync(dto.BondFinId);
        var swap = await _fiRepo.GetByIdAsync(dto.SwapFinId);

        if (bond is null)
            return (null, $"Bond with FIN_ID {dto.BondFinId} not found.", 404);

        if (swap is null)
            return (null, $"Swap with FIN_ID {dto.SwapFinId} not found.", 404);

        // 2. Verify deal types
        if (!bond.IsBond())
            return (null, $"FIN_ID {dto.BondFinId} is not a BOND instrument.", 400);

        if (!swap.IsSwap())
            return (null, $"FIN_ID {dto.SwapFinId} is not a SWAP instrument.", 400);

        // 3. Verify both are Active
        if (!bond.IsActive())
            return (null, $"Bond {bond.DealNumber} is Inactive and cannot be paired.", 400);

        if (!swap.IsActive())
            return (null, $"Swap {swap.DealNumber} is Inactive and cannot be paired.", 400);

        // 4. Validate notional amounts match (DECIMAL equality)
        if (bond.NotionalAmt != swap.NotionalAmt)
            return (null,
                $"Notional Amount mismatch: Bond ({bond.NotionalAmt:N2}) ≠ Swap ({swap.NotionalAmt:N2}). " +
                "Selected Values does not match the required amount.",
                400);

        // 5. Check neither instrument is already in an existing pair
        if (await _fiRepo.IsAlreadyPairedAsync(dto.BondFinId))
            return (null, $"Bond {bond.DealNumber} is already assigned to an existing Hedge Pair.", 409);

        if (await _fiRepo.IsAlreadyPairedAsync(dto.SwapFinId))
            return (null, $"Swap {swap.DealNumber} is already assigned to an existing Hedge Pair.", 409);

        // 6. Persist
        var hedgePair = new API.Models.HedgePair
        {
            BondFinId = dto.BondFinId,
            SwapFinId = dto.SwapFinId,
            BondNotionalAmt = bond.NotionalAmt,
            SwapNotionalAmt = swap.NotionalAmt
        };

        var created = await _hpRepo.CreateAsync(hedgePair);
        _logger.LogInformation("Hedge Pair {Id} created: Bond {Bond} ↔ Swap {Swap}",
            created.HedgePairId, bond.DealNumber, swap.DealNumber);

        return (ToDto(created), null, 201);
    }

    // ── Delete ───────────────────────────────────────────────────────────────

    public async Task<(bool Success, string? Error)> DeleteAsync(int hedgePairId)
    {
        var pair = await _hpRepo.GetByIdAsync(hedgePairId);
        if (pair is null)
            return (false, $"Hedge Pair with ID {hedgePairId} not found.");

        await _hpRepo.DeleteAsync(pair);
        _logger.LogInformation("Hedge Pair {Id} deleted.", hedgePairId);
        return (true, null);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static HedgePairDto ToDto(API.Models.HedgePair hp) => new(
        hp.HedgePairId,
        hp.BondFinId,
        hp.Bond?.DealNumber ?? string.Empty,
        hp.BondNotionalAmt,
        hp.SwapFinId,
        hp.Swap?.DealNumber ?? string.Empty,
        hp.SwapNotionalAmt
    );
}
