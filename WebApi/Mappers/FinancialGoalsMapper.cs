using AutoMapper;
using WebApi.Dtos;
using WebApi.Model;

namespace WebApi.Mappers;

public class FinancialGoalsMapper : Profile
{
    public FinancialGoalsMapper()
    {
        CreateMap<FinancialGoal, ListFinancialGoalResponse>();
        CreateMap<AddFinancialGoalRequest, FinancialGoal>();
        CreateMap<FinancialGoal, AddFinancialGoalResponse>();
        CreateMap<EditFinancialGoalRequest, FinancialGoal>();
        CreateMap<FinancialGoal, EditFinancialGoalResponse>();

        CreateMap<FinancialGoal, FinancialGoal>()
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
