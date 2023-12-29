using AutoMapper;
using WebApi.Mappers;

namespace UnitTests.Mapper;

public static class MapperFactory
{
    public static IMapper GetMapperConfig()
    {
        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<FinancialGoalsMapper>();
            cfg.AddProfile<TransactionsMapper>();
            cfg.AddProfile<PeriodMapper>();
            cfg.AddProfile<EnumMapper>();
            cfg.AddProfile<CategoriesMapper>();
        });

        return config.CreateMapper();
    }    
}
