using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IExternalProductRepository _productRepository;
    private readonly IExternalCustomerRepository _customerRepository;
    private readonly IExternalBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates mock objects.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _productRepository = Substitute.For<IExternalProductRepository>();
        _customerRepository = Substitute.For<IExternalCustomerRepository>();
        _branchRepository = Substitute.For<IExternalBranchRepository>();
        _mapper = Substitute.For<IMapper>();

        // Add a mock for ISaleCreateRepository
        var saleCreateRepository = Substitute.For<ISaleCreateRepository>();

        _handler = new CreateSaleHandler(_saleRepository, _mapper, _productRepository, _customerRepository, _branchRepository, saleCreateRepository);
    }

    /// <summary>
    /// Tests that a valid sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var customer = ExternalCustomerTestData.GenerateValidExternalCustomer();
        var branch = ExternalBranchTestData.GenerateValidExternalBranch();
        var product = ExternalProductTestData.GenerateValidExternalProduct();
        var sale = new Sale(customer.Id, branch.Id, command.SaleNumber, DateTime.Now, customer, branch);
        var result = new CreateSaleResult(sale.Id);

        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(product);
        _customerRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(branch);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(sale.Id);
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = string.Empty,  // Invalid SaleNumber
            SaleDate = DateTime.MinValue,  // Invalid SaleDate
            Customer = ExternalCustomerTestData.GenerateInvalidExternalCustomer(),  // Invalid Customer (assumindo que ele não pode ser null)
            Branch = ExternalBranchTestData.GenerateInvalidExternalBranch(),  // Invalid Branch (assumindo que ele não pode ser null)
            SaleItems = new List<SaleItem>(),  // SaleItems como lista vazia
            TotalAmount = 0,  // Invalid TotalAmount
            IsCancelled = false
        };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the customer and branch are created if not provided.
    /// </summary>
    [Fact(DisplayName = "Given new customer and branch When handling Then creates them")]
    public async Task Handle_NewCustomerAndBranch_CreatesEntities()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var customer = ExternalCustomerTestData.GenerateValidExternalCustomer();
        var branch = ExternalBranchTestData.GenerateValidExternalBranch();
        var product = ExternalProductTestData.GenerateValidExternalProduct();
        command.Customer.Id = Guid.Empty;
        command.Branch.Id = Guid.Empty;
        foreach (var item in command.SaleItems)
        {
            item.Product.Id = Guid.Empty;
        }

        _customerRepository.CreateAsync(Arg.Any<ExternalCustomer>(), Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.CreateAsync(Arg.Any<ExternalBranch>(), Arg.Any<CancellationToken>()).Returns(branch);
        _productRepository.CreateAsync(Arg.Any<ExternalProduct>(), Arg.Any<CancellationToken>()).Returns(product);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _customerRepository.Received(1).CreateAsync(Arg.Any<ExternalCustomer>(), Arg.Any<CancellationToken>());
        await _branchRepository.Received(1).CreateAsync(Arg.Any<ExternalBranch>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called to map the sale to the result.
    /// </summary>
    [Fact(DisplayName = "Given valid sale When handling Then maps sale to result")]
    public async Task Handle_ValidRequest_MapsSaleToResult()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var customer = ExternalCustomerTestData.GenerateValidExternalCustomer();
        var branch = ExternalBranchTestData.GenerateValidExternalBranch();
        var product = ExternalProductTestData.GenerateValidExternalProduct();
        var sale = new Sale(customer.Id, branch.Id, command.SaleNumber, DateTime.Now, customer, branch);
        _customerRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(customer);
        _branchRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(branch);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(product);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<CreateSaleResult>(sale);
    }
}