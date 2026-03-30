using CoolShool.Application.Contracts.Requests;
using CoolShool.Application.Contracts.Responses;
using CoolShool.Application.Interfaces;

namespace CoolShool.WebApi.GraphQL;

public class Mutation
{
    public async Task<FinancialOwnerResponse> CreateFinancialOwner(
        CreateFinancialOwnerRequest input, 
        [Service] IFinancialOwnerService service)
    {
        var result = await service.CreateAsync(input);
        if (result.IsFailure) throw new GraphQLException(result.Error ?? "Erro desconhecido");
        return result.Data!;
    }

    public async Task<PaymentPlanResponse> CreatePaymentPlan(
        CreatePaymentPlanRequest input,
        [Service] IPaymentPlanService service)
    {
        var result = await service.CreateAsync(input);
        if (result.IsFailure) throw new GraphQLException(result.Error ?? "Erro desconhecido");
        return result.Data!;
    }

    public async Task<FinancialOwnerResponse> UpdateFinancialOwner(
        long id,
        UpdateFinancialOwnerRequest input,
        [Service] IFinancialOwnerService service)
    {
        var result = await service.UpdateAsync(id, input);
        if (result.IsFailure) throw new GraphQLException(result.Error ?? "Erro desconhecido");
        return result.Data!;
    }

    public async Task<bool> DeleteFinancialOwner(
        long id,
        [Service] IFinancialOwnerService service)
    {
        var result = await service.DeleteAsync(id);
        if (result.IsFailure) throw new GraphQLException(result.Error ?? "Erro desconhecido");
        return true;
    }

    public async Task<CostCenterResponse> CreateCostCenter(
        CreateCostCenterRequest input,
        [Service] ICostCenterService service)
    {
        var result = await service.CreateAsync(input);
        if (result.IsFailure) throw new GraphQLException(result.Error ?? "Erro desconhecido");
        return result.Data!;
    }

    public async Task<CostCenterResponse> UpdateCostCenter(
        long id,
        UpdateCostCenterRequest input,
        [Service] ICostCenterService service)
    {
        var result = await service.UpdateAsync(id, input);
        if (result.IsFailure) throw new GraphQLException(result.Error ?? "Erro desconhecido");
        return result.Data!;
    }

    public async Task<bool> DeleteCostCenter(
        long id,
        [Service] ICostCenterService service)
    {
        var result = await service.DeleteAsync(id);
        if (result.IsFailure) throw new GraphQLException(result.Error ?? "Erro desconhecido");
        return true;
    }

    public async Task<bool> RegisterPayment(
        long billingId,
        RegisterPaymentRequest input,
        [Service] IBillingService service)
    {
        var result = await service.RegisterPaymentAsync(billingId, input);
        if (result.IsFailure) throw new GraphQLException(result.Error ?? "Erro desconhecido");
        return true;
    }

    public async Task<bool> CancelCharge(
        long billingId,
        [Service] IBillingService service)
    {
        var result = await service.CancelAsync(billingId);
        if (result.IsFailure) throw new GraphQLException(result.Error ?? "Erro desconhecido");
        return true;
    }
}
