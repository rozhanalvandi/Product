using AutoMapper;
using Product.Models;
using Product.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductDto, Products>();
        CreateMap<Products, ProductDto>();
    }
}