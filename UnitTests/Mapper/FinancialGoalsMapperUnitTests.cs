using AutoMapper;
using FluentAssertions;
using WebApi.Dtos;
using WebApi.Dtos.Builders;
using WebApi.Model;
using WebApi.Model.Builders;

namespace UnitTests.Mapper
{
    public class FinancialGoalsMapperUnitTests
    {
        private readonly IMapper _mapper;

        public FinancialGoalsMapperUnitTests()
        {
            _mapper = MapperFactory.GetMapperConfig();
        }

        [Fact]
        public void FinancialGoal_To_ListFinancialGoalResponse_ShouldMap()
        {
            var financialGoal = FinancialGoalBuilder
                                .New
                                .WithId(Guid.NewGuid())
                                .WithCategory(CategoryBuilder
                                                .New
                                                .WithId(Guid.NewGuid())
                                                .WithName("Category 123")
                                                .WithIsEssential(true)
                                                .Build())
                                .WithCategoryId(Guid.NewGuid())
                                .WithPeriod(PeriodBuilder
                                                .New
                                                .WithId(Guid.NewGuid())
                                                .WithStart(DateOnly.FromDateTime(DateTime.Now.AddDays(-3)))
                                                .WithEnd(DateOnly.FromDateTime(DateTime.Now.AddDays(3)))
                                                .WithName("123456")
                                                .Build())
                                .WithPeriodId(Guid.NewGuid())
                                .WithGoal(213.09m)
                                .Build();

            var listFinancialGoalResponse = _mapper.Map<ListFinancialGoalResponse>(financialGoal);

            listFinancialGoalResponse.Id.Should().Be(financialGoal.Id);
            listFinancialGoalResponse.Goal.Should().Be(financialGoal.Goal);

            listFinancialGoalResponse.Category.Name.Should().Be(financialGoal.Category.Name);
            listFinancialGoalResponse.Category.IsEssential.Should().Be(financialGoal.Category.IsEssential);
            listFinancialGoalResponse.Category.Id.Should().Be(financialGoal.Category.Id);

            listFinancialGoalResponse.Period.Id.Should().Be(financialGoal.Period.Id);
            listFinancialGoalResponse.Period.Name.Should().Be(financialGoal.Period.Name);
        }

        [Fact]
        public void AddFinancialGoalRequest_To_FinancialGoal_ShouldMap()
        {
            var addFinancialGoalRequest = AddFinancialGoalRequestBuilder
                                            .New
                                            .WithCategoryId(Guid.NewGuid())                                
                                            .WithPeriodId(Guid.NewGuid())
                                            .WithGoal(213.09m)
                                            .Build();

            var financialGoal = _mapper.Map<FinancialGoal>(addFinancialGoalRequest);

            financialGoal.CategoryId.Should().Be(addFinancialGoalRequest.CategoryId);
            financialGoal.PeriodId.Should().Be(addFinancialGoalRequest.PeriodId);
            financialGoal.Goal.Should().Be(addFinancialGoalRequest.Goal);
        }

        [Fact]
        public void FinancialGoal_To_AddFinancialGoalResponse_ShouldMap()
        {
            var financialGoal = FinancialGoalBuilder
                                .New
                                .WithId(Guid.NewGuid())
                                .WithCategory(CategoryBuilder
                                                .New
                                                .WithId(Guid.NewGuid())
                                                .WithName("Category 123")
                                                .WithIsEssential(true)
                                                .Build())
                                .WithCategoryId(Guid.NewGuid())
                                .WithPeriod(PeriodBuilder
                                                .New
                                                .WithId(Guid.NewGuid())
                                                .WithStart(DateOnly.FromDateTime(DateTime.Now.AddDays(-3)))
                                                .WithEnd(DateOnly.FromDateTime(DateTime.Now.AddDays(3)))
                                                .WithName("123456")
                                                .Build())
                                .WithPeriodId(Guid.NewGuid())
                                .WithGoal(213.09m)
                                .Build();

            var addFinancialGoalResponse = _mapper.Map<AddFinancialGoalResponse>(financialGoal);

            addFinancialGoalResponse.Id.Should().Be(financialGoal.Id);
            addFinancialGoalResponse.Goal.Should().Be(financialGoal.Goal);

            addFinancialGoalResponse.Category.Name.Should().Be(financialGoal.Category.Name);
            addFinancialGoalResponse.Category.IsEssential.Should().Be(financialGoal.Category.IsEssential);
            addFinancialGoalResponse.Category.Id.Should().Be(financialGoal.Category.Id);

            addFinancialGoalResponse.Period.Id.Should().Be(financialGoal.Period.Id);
            addFinancialGoalResponse.Period.Name.Should().Be(financialGoal.Period.Name);
        }

        [Fact]
        public void EditFinancialGoalRequest_To_FinancialGoal_ShouldMap()
        {
            var editFinancialGoalRequest = EditFinancialGoalRequestBuilder
                                            .New
                                            .WithGoal(213.09m)
                                            .Build();

            var financialGoal = _mapper.Map<FinancialGoal>(editFinancialGoalRequest);

            financialGoal.Goal.Should().Be(editFinancialGoalRequest.Goal);
        }

        [Fact]
        public void FinancialGoal_To_EditFinancialGoalResponse_ShouldMap()
        {
            var financialGoal = FinancialGoalBuilder
                                .New
                                .WithId(Guid.NewGuid())
                                .WithCategory(CategoryBuilder
                                                .New
                                                .WithId(Guid.NewGuid())
                                                .WithName("Category 123")
                                                .WithIsEssential(true)
                                                .Build())
                                .WithCategoryId(Guid.NewGuid())
                                .WithPeriod(PeriodBuilder
                                                .New
                                                .WithId(Guid.NewGuid())
                                                .WithStart(DateOnly.FromDateTime(DateTime.Now.AddDays(-3)))
                                                .WithEnd(DateOnly.FromDateTime(DateTime.Now.AddDays(3)))
                                                .WithName("123456")
                                                .Build())
                                .WithPeriodId(Guid.NewGuid())
                                .WithGoal(213.09m)
                                .Build();

            var editFinancialGoalResponse = _mapper.Map<EditFinancialGoalResponse>(financialGoal);

            editFinancialGoalResponse.Id.Should().Be(financialGoal.Id);
            editFinancialGoalResponse.Goal.Should().Be(financialGoal.Goal);

            editFinancialGoalResponse.Category.Name.Should().Be(financialGoal.Category.Name);
            editFinancialGoalResponse.Category.IsEssential.Should().Be(financialGoal.Category.IsEssential);
            editFinancialGoalResponse.Category.Id.Should().Be(financialGoal.Category.Id);

            editFinancialGoalResponse.Period.Id.Should().Be(financialGoal.Period.Id);
            editFinancialGoalResponse.Period.Name.Should().Be(financialGoal.Period.Name);
        }
    }
}
