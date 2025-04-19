using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

/// <summary>
/// Mapping profile for CreateSale operations.
/// </summary>
public class SaleProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaleProfile"/> class.
    /// </summary>
    public SaleProfile()
    {
        // Map from CreateSaleRequest to CreateSaleCommand
        CreateMap<CreateSaleRequest, CreateSaleCommand>()
            .ForMember(dest => dest.SaleItems, opt => opt.MapFrom(src => src.Products))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch));

        // Map from CreateSaleProductsRequest to SaleItem
        CreateMap<CreateSaleProductsRequest, SaleItem>()
             .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
             .ForPath(dest => dest.Product.Id, opt => opt.MapFrom(src => src.ProductId))
             .ForPath(dest => dest.Product.ProductName, opt => opt.MapFrom(src => src.ProductName))
             .ForPath(dest => dest.Product.Price, opt => opt.MapFrom(src => src.Price))
             .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
             .ForMember(dest => dest.Discount, opt => opt.Ignore()) // Calculated in business logic
             .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()); // Calculated in business logic


        // Map from CreateSaleCustomerRequest to ExternalCustomer
        CreateMap<CreateSaleCustomerRequest, ExternalCustomer>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));

        // Map from CreateSaleBranchRequest to ExternalBranch
        CreateMap<CreateSaleBranchRequest, ExternalBranch>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BranchId))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));

        CreateMap<CreateSaleResult, CreateSaleResponse>();

        CreateMap<GetSaleRequest, GetSaleQuery>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id));

        CreateMap<Sale, GetSaleResult>();

        CreateMap<GetSaleResult, GetSaleResponse>();
    }
}