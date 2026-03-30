using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;
using CoolShool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoolShool.WebApi.Controllers;


[ApiController]
[Route("api/centros-de-custo")]
public sealed class CostCentersController(ICostCenterService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CostCenterResponse>> Create(CreateCostCenterRequest request)
    {
        var result = await service.CreateAsync(request);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Data);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CostCenterResponse>>> GetAll()
    {
        var result = await service.GetAllAsync();
        return Ok(result.Data);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<CostCenterResponse>> Update(long id, UpdateCostCenterRequest request)
    {
        var result = await service.UpdateAsync(id, request);
        if (result.IsFailure) return BadRequest(result.Error);
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
