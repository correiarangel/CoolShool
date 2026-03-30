using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;
using CoolShool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoolShool.WebApi.Controllers;

[ApiController]
[Route("api/planos-de-pagamento")]
public sealed class PaymentPlansController(IPaymentPlanService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PaymentPlanResponse>> Create(CreatePaymentPlanRequest request)
    {
        var result = await service.CreateAsync(request);
        if (result.IsFailure) return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentPlanResponse>> GetById(long id)
    {
        var result = await service.GetByIdAsync(id);
        if (result.IsFailure) return NotFound(result.Error);

        return Ok(result.Data);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await service.DeleteAsync(id);
        if (result.IsFailure) return BadRequest(result.Error);

        return NoContent();
    }
}
