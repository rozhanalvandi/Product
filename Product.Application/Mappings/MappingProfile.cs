using AutoMapper;
using Product.Domain.Models;
using Product.Application.DTOs;

namespace Product.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, Products>();
            CreateMap<Products, ProductDto>();
        }
    }
}
