using AsynchronousProgrammingMVC.Models.DTO_s.CategoryDTO;
using AsynchronousProgrammingMVC.Models.DTO_s.ProductDTO;
using AsynchronousProgrammingMVC.Models.Entities.Concrete;
using AutoMapper;

namespace AsynchronousProgrammingMVC.Infrastructure.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Category, AddCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();

            CreateMap<Product, AddProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();
        }
    }
}
