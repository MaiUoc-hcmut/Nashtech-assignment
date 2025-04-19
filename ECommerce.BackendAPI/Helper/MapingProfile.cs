using AutoMapper;
using Ecommerce.SharedViewModel.Models;
using Ecommerce.SharedViewModel.DTOs;
using Ecommerce.SharedViewModel.ParametersType;


namespace Ecommerce.BackendAPI.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Variant, VariantDTO>().ReverseMap();
            CreateMap<Variant, CreateVariantsOfProductParameter>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<ParentCategory, ParentCategoryDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
        }
    }
}