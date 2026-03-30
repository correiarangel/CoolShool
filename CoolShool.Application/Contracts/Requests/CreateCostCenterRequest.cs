using CoolShool.Domain.Enums;

namespace CoolShool.Application.Contracts.Requests;

public sealed record CreateCostCenterRequest(CostCenterType Type, string? Name);
