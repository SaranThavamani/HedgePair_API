using HedgePair.API.Models;

namespace HedgePair.API.Interfaces;

public interface IFinancialInstrumentRepository
{
    /// <summary>Returns all Active instruments that are NOT already in any Hedge Pair.</summary>
    Task<IEnumerable<FinancialInstrument>> GetActiveUnpairedAsync();

    /// <summary>Returns a single instrument by primary key, or null if not found.</summary>
    Task<FinancialInstrument?> GetByIdAsync(int finId);

    /// <summary>Returns true when the given FIN_ID appears in any existing Hedge Pair.</summary>
    Task<bool> IsAlreadyPairedAsync(int finId);
}

public interface IHedgePairRepository
{
    /// <summary>Returns all Hedge Pairs with their Bond and Swap navigation properties loaded.</summary>
    Task<IEnumerable<HedgePair.API.Models.HedgePair>> GetAllAsync();

    /// <summary>Returns a Hedge Pair by its primary key, or null if not found.</summary>
    Task<HedgePair.API.Models.HedgePair?> GetByIdAsync(int hedgePairId);

    /// <summary>Persists a new Hedge Pair and returns the saved entity.</summary>
    Task<HedgePair.API.Models.HedgePair> CreateAsync(HedgePair.API.Models.HedgePair hedgePair);

    /// <summary>Hard-deletes the specified Hedge Pair.</summary>
    Task DeleteAsync(HedgePair.API.Models.HedgePair hedgePair);
}
