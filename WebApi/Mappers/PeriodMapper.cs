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

        CreateMap<Period, ListPeriodsResponse>()
            .ForMember(m => m.IsCurrent, (x) =>
                x.MapFrom((src, dest) => Today() >= src.Start && Today() <= src.End));

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

    
    private static DateOnly Today() => DateOnly.FromDateTime(DateTime.UtcNow);
}
