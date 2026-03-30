using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;
using CoolShool.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CoolShool.WebApi.Controllers;


[ApiController]
[Route("api/responsaveis")]
public sealed class FinancialOwnersController(IFinancialOwnerService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<FinancialOwnerResponse>> Create(CreateFinancialOwnerRequest request)
    {
        var result = await service.CreateAsync(request);
        if (result.IsFailure) return BadRequest(result.Error);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FinancialOwnerResponse>>> GetAll()
    {
        var result = await service.GetAllAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FinancialOwnerResponse>> GetById(long id)
    {
        var result = await service.GetByIdAsync(id);
        if (result.IsFailure) return NotFound(result.Error);
        
        return Ok(result.Data);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<FinancialOwnerResponse>> Update(long id, UpdateFinancialOwnerRequest request)
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