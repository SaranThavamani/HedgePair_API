namespace HedgePair.API.Models;

/// <summary>Financial Instrument master data entity (Bond or Swap).</summary>
public class FinancialInstrument
{
    public int FinId { get; set; }
    public string DealNumber { get; set; } = string.Empty;
    public decimal NotionalAmt { get; set; }
    public string DealType { get; set; } = string.Empty;   // "BOND" | "SWAP"
    public string DealStatus { get; set; } = string.Empty;   // "Active" | "Inactive"

    // Navigation – an FI can be the Bond side of at most one pair
    public HedgePair? BondPair { get; set; }
    // Navigation – an FI can be the Swap side of at most one pair
    public HedgePair? SwapPair { get; set; }

    public bool IsActive() => DealStatus == "Active";
    public bool IsBond() => DealType == "BOND";
    public bool IsSwap() => DealType == "SWAP";
}
