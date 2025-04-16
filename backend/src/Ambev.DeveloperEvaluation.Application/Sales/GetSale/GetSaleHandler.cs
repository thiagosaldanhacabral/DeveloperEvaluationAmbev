using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Commom;
using Ambev.DeveloperEvaluation.Application.Interfaces;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetSaleQuery requests
/// </summary>
public class GetSaleHandler(
    ISaleRepository saleRepository,
    ISaleQueryRepository saleQueryRepository,
    IMapper mapper,
    IValidator<GetSaleQuery> validator) : IRequestHandler<GetSaleQuery, GetSaleResult>
{
    private readonly ISaleRepository _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
    private readonly ISaleQueryRepository _saleQueryRepository = saleQueryRepository ?? throw new ArgumentNullException(nameof(saleQueryRepository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IValidator<GetSaleQuery> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

    public async Task<GetSaleResult> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        // Validate the request
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Attempt to retrieve the sale from the cache or repository
        var sale = await GetSaleFromCacheOrRepositoryAsync(request.Id, cancellationToken);

        // Map the sale entity to the result DTO
        return _mapper.Map<GetSaleResult>(sale);
    }

    private async Task<Sale> GetSaleFromCacheOrRepositoryAsync(Guid saleId, CancellationToken cancellationToken)
    {
        var parameters = new QueryParams<Sale>
        {
            Filter = s => s.Id.Equals(saleId)
        };

        var saleCache = await _saleQueryRepository.QueryAsync(parameters, cancellationToken);
        return saleCache?.FirstOrDefault()
            ?? await _saleRepository.GetByIdAsync(saleId, cancellationToken)
            ?? throw new KeyNotFoundException($"Sale with ID {saleId} not found");
    }
}
