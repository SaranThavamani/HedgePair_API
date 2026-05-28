namespace HedgePair.API.Models;

/// <summary>Represents a paired Bond–Swap Hedge Pair record.</summary>
public class HedgePair
{
    public int HedgePairId { get; set; }
    public int BondFinId { get; set; }
    public int SwapFinId { get; set; }
    public decimal BondNotionalAmt { get; set; }
    public decimal SwapNotionalAmt { get; set; }

    // Navigation properties
    public FinancialInstrument? Bond { get; set; }
    public FinancialInstrument? Swap { get; set; }
}
