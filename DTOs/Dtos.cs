namespace HedgePair.API.DTOs;

// ── Request DTOs ─────────────────────────────────────────────────────────────

/// <summary>Request body for creating a Hedge Pair.</summary>
public record CreatePairDto(int BondFinId, int SwapFinId);

// ── Response DTOs ────────────────────────────────────────────────────────────

/// <summary>Financial Instrument data returned to the UI dropdowns.</summary>
public record FinancialInstrumentDto(
    int FinId,
    string DealNumber,
    decimal NotionalAmt,
    string DealType,
    string DealStatus
);

/// <summary>Full Hedge Pair details returned to the UI table.</summary>
public record HedgePairDto(
    int HedgePairId,
    int BondFinId,
    string BondDealNumber,
    decimal BondNotionalAmt,
    int SwapFinId,
    string SwapDealNumber,
    decimal SwapNotionalAmt
);

/// <summary>Standard error response following RFC 7807 ProblemDetails pattern.</summary>
public record ErrorDto(string Message);
