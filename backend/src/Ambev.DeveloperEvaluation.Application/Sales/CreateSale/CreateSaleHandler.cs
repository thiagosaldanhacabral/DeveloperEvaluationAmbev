using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Interfaces;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler(
    ISaleRepository saleRepository,
    IMapper mapper,
    IExternalProductRepository productRepository,
    IExternalCustomerRepository customerRepository,
    IExternalBranchRepository branchRepository,
    ISaleCreateRepository saleCreateRepository
) : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the created sale</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        // Validate the CreateSaleCommand
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Handle customer creation or retrieval
        var customer = command.Customer.Id == Guid.Empty
            ? await customerRepository.CreateAsync(command.Customer, cancellationToken)
            : await customerRepository.GetByIdAsync(command.Customer.Id, cancellationToken);

        // Handle branch creation or retrieval
        var branch = command.Branch.Id == Guid.Empty
            ? await branchRepository.CreateAsync(command.Branch, cancellationToken)
            : await branchRepository.GetByIdAsync(command.Branch.Id, cancellationToken);

        // Create the sale
        var sale = new Sale(customer?.Id ?? Guid.NewGuid(), branch?.Id ?? Guid.NewGuid(), command.SaleNumber, command.SaleDate, customer, branch);

        // Add sale items
        foreach (var item in command.SaleItems)
        {
            var product = item.Product.Id == Guid.Empty
                ? await productRepository.CreateAsync(item.Product, cancellationToken)
                : await productRepository.GetByIdAsync(item.ProductId, cancellationToken);

            if (product == null)
                throw new InvalidOperationException($"Product with ID {item.ProductId} could not be found or created.");

            var saleItem = new SaleItem(Guid.Empty, item.Quantity, product);
            sale.AddItem(saleItem);
        }

        // Recalculate total amount and save the sale
        sale.RecalculateTotalAmount();
        var createdSale = await saleRepository.CreateAsync(sale, cancellationToken);

        // Update the sale in the repository
        await saleRepository.UpdateAsync(createdSale, cancellationToken);

        // Store the sale in the cache
        var cacheKey = $"Sale:{createdSale.Id}";
        await saleCreateRepository.StoreSaleInCacheAsync(createdSale, cacheKey, cancellationToken);

        // Map and return the result
        return mapper.Map<CreateSaleResult>(createdSale);
    }
}
