using AutoMapper;
using WebApi.Model;

namespace WebApi.Mappers;

public class EnumMapper : Profile
{
    public EnumMapper()
    {
        CreateMap<TransactionType?, TransactionType>()
            .ConvertUsing(source => source ?? TransactionType.Unknown);
    }
}
