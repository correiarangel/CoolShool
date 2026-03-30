using CoolShool.Domain.Enums;

namespace CoolShool.Application.Contracts.Responses;

public sealed record CostCenterResponse(long Id, CostCenterType Type, string? Name, string DisplayName);
