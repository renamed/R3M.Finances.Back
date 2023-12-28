using AutoMapper;
using WebApi.Mappers;

namespace UnitTests.Mapper;

public static class MapperFactory
{
    public static IMapper GetCategoriesMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CategoriesMapper>());
        return config.CreateMapper();
    }
}
