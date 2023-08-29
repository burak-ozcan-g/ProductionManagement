using AutoMapper;
using ProductionManagement.Dto;
using ProductionManagement.Models;

namespace ProductionManagement.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<Package, PackageDto>();
            CreateMap<PackageDto, Package>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
