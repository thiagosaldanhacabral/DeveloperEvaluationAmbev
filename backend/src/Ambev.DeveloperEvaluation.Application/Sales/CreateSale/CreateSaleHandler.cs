using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IExternalProductRepository _productRepository;
    private readonly IExternalCustomerRepository _customerRepository;
    private readonly IExternalBranchRepository _branchRepository;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="customerRepository">The customer repository</param>
    /// <param name="branchRepository">The branch repository</param>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IExternalProductRepository productRepository,
        IExternalCustomerRepository customerRepository,
        IExternalBranchRepository branchRepository)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
    }

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

        var customer = command.Customer;
        var branch = command.Branch;

        if (customer.Id.ToString() == Guid.Empty.ToString())
        {
            customer = await _customerRepository.CreateAsync(customer, cancellationToken);
        }
        else
        {
            customer = await _customerRepository.GetByIdAsync(customer.Id, cancellationToken);
        }

        if (branch.Id.ToString() == Guid.Empty.ToString())
        {
            branch = await _branchRepository.CreateAsync(branch, cancellationToken);
        }
        else
        {
            branch = await _branchRepository.GetByIdAsync(command.Branch.Id, cancellationToken);
        }

        var sale = new Sale(customer?.Id?? Guid.NewGuid(), branch?.Id?? Guid.NewGuid(), command.SaleNumber, command.SaleDate, customer, branch);

        foreach (var item in command.SaleItems)
        {
            var product = item.Product;
            if (product.Id.ToString() == Guid.Empty.ToString())
            {
                product = await _productRepository.CreateAsync(product, cancellationToken);
            }
            else
            {
                product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            }

            var saleItem = new SaleItem(Guid.Empty, item.Quantity, product);
            sale.AddItem(saleItem);
        }
        sale.RecalculateTotalAmount();
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        await _saleRepository.UpdateAsync(createdSale, cancellationToken);

        var result = _mapper.Map<CreateSaleResult>(createdSale);
        return result;
    }
}