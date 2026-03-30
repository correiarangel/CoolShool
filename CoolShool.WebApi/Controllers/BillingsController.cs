using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;
using CoolShool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoolShool.WebApi.Controllers;

[ApiController]
[Route("api/cobrancas")]
public sealed class BillingsController(IBillingService service) : ControllerBase
{
    [HttpGet("responsavel/{ownerId}")]
    public async Task<ActionResult<IEnumerable<BillingResponse>>> GetByOwner(long ownerId)
    {
        var result = await service.GetByOwnerAsync(ownerId);
        return Ok(result.Data);
    }

    [HttpGet("responsavel/{ownerId}/quantidade")]
    public async Task<ActionResult<int>> GetCountByOwner(long ownerId)
    {
        var result = await service.GetCountByOwnerAsync(ownerId);
        return Ok(result.Data);
    }

    [HttpPost("{id}/pagamentos")]
    public async Task<IActionResult> RegisterPayment(long id, RegisterPaymentRequest request)
    {
        var result = await service.RegisterPaymentAsync(id, request);
        if (result.IsFailure) return BadRequest(result.Error);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await service.DeleteAsync(id);
        if (result.IsFailure) return BadRequest(result.Error);
        
        return NoContent();
    }
}
