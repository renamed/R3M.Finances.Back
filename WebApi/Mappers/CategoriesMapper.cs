using AutoMapper;
using WebApi.Dtos;
using WebApi.Model;

namespace WebApi.Mappers;

public class CategoriesMapper : Profile
{
    public CategoriesMapper()
    {
        CreateMap<Category, ListCategoriesResponse>();

        CreateMap<AddCategoryRequest, Category>();
        CreateMap<Category, AddCategoryResponse>();

        CreateMap<EditCategoryRequest, Category>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            

        CreateMap<Category, EditCategoryResponse>();

        CreateMap<Category, DefaultCategoryResponse>();
    }
}
