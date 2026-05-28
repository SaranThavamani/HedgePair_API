using HedgePair.API.DTOs;
using HedgePair.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace HedgePair.API.Controllers;

/// <summary>Manages the Hedge Pair lifecycle: Create, Read All, Delete.</summary>
[ApiController]
[Route("api/pairing")]
[Produces("application/json")]
public class HedgePairController : ControllerBase
{
    private readonly IHedgePairService _service;

    public HedgePairController(IHedgePairService service) => _service = service;

    // ── GET /api/pairing/all ──────────────────────────────────────────────────

    /// <summary>Returns all existing Hedge Pairs for the UI display table.</summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<HedgePairDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var pairs = await _service.GetAllAsync();
        return Ok(pairs);
    }

    // ── POST /api/pairing ─────────────────────────────────────────────────────

    /// <summary>
    /// Creates a new Hedge Pair by associating a Bond with a Swap.
    /// Validates: existence, Active status, notional equality, and no duplicate pairing.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(HedgePairDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreatePairDto dto)
    {
        if (dto.BondFinId <= 0 || dto.SwapFinId <= 0)
            return BadRequest(new ErrorDto("BondFinId and SwapFinId must be positive integers."));

        var (result, error, statusCode) = await _service.CreateAsync(dto);

        return statusCode switch
        {
            201 => CreatedAtAction(nameof(GetAll), result),
            400 => BadRequest(new ErrorDto(error!)),
            404 => NotFound(new ErrorDto(error!)),
            409 => Conflict(new ErrorDto(error!)),
            _ => StatusCode(500, new ErrorDto("An unexpected error occurred."))
        };
    }

    // ── DELETE /api/pairing/{id} ──────────────────────────────────────────────

    /// <summary>
    /// Permanently deletes a Hedge Pair by its HEDGE_PAIR_ID.
    /// The Bond and Swap instruments are returned to the available pool.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound(new ErrorDto(error!));
    }
}
