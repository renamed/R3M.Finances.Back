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

    public static IMapper GetEnumMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<EnumMapper>());
        return config.CreateMapper();
    }

    public static IMapper GetPeriodsMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<PeriodMapper>());
        return config.CreateMapper();
    }

    public static IMapper GetTransactionsMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<TransactionsMapper>());
        return config.CreateMapper();
    }
}
