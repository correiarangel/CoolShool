using CoolShool.Domain.Enums;

namespace CoolShool.Application.Contracts.Requests;

public record UpdateCostCenterRequest(CostCenterType Type, string? Name);
