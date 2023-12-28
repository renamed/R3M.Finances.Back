using AutoMapper;
using WebApi.Dtos;
using WebApi.Model;

namespace WebApi.Mappers;

public class PeriodMapper : Profile
{
    public PeriodMapper()
    {
        CreateMap<AddPeriodRequest, Period>();
        CreateMap<Period, AddPeriodResponse>();
        CreateMap<Period, DefaultPeriodResponse>();
        CreateMap<Period, ListPeriodsResponse>();

        CreateMap<EditPeriodRequest, Period>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Period, EditPeriodResponse>();

        CreateMap<Period, Period>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
            {
                if (srcMember == null)
                    return false;

                if (srcMember.GetType() == typeof(Guid))
                {
                    return (Guid)srcMember != default;
                }

                if (srcMember.GetType() == typeof(DateOnly))
                {
                    return (DateOnly)srcMember != default;
                }

                return true;
            }));

    }
}
