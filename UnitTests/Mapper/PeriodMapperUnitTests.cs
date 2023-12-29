using AutoMapper;
using FluentAssertions;
using WebApi.Dtos;
using WebApi.Dtos.Builders;
using WebApi.Model;
using WebApi.Model.Builders;

namespace UnitTests.Mapper
{
    public class PeriodMapperUnitTests
    {
        private readonly IMapper _mapper;

        public PeriodMapperUnitTests()
        {
            _mapper = MapperFactory.GetPeriodsMapper();
        }

        [Fact]
        public void AddPeriodRequest_To_Period()
        {
            var addPeriodRequest = AddPeriodsRequestBuilder
                                    .New()
                                    .WithStart(new DateOnly(2023, 12, 1))
                                    .WithEnd(new DateOnly(2023, 12, 31))
                                    .WithName("Random name")
                                    .Build();

            var period = _mapper.Map<Period>(addPeriodRequest);

            period.Start.Should().Be(addPeriodRequest.Start);
            period.End.Should().Be(addPeriodRequest.End);
            period.Name.Should().Be(addPeriodRequest.Name);
        }

        [Fact]
        public void Period_To_AddPeriodResponse()
        {
            var period = PeriodBuilder
                            .New
                            .WithStart(new DateOnly(2023, 12, 1))
                            .WithEnd(new DateOnly(2023, 12, 31))
                            .WithId(Guid.NewGuid())
                            .WithName("Period name")
                            .Build();

            var addPeriodResponse = _mapper.Map<AddPeriodResponse>(period);

            addPeriodResponse.Start.Should().Be(period.Start);
            addPeriodResponse.End.Should().Be(period.End);
            addPeriodResponse.Name.Should().Be(period.Name);
            addPeriodResponse.Id.Should().Be(period.Id);
        }

        [Fact]
        public void Period_To_DefaultPeriodResponse()
        {
            var period = PeriodBuilder
                            .New
                            .WithStart(new DateOnly(2023, 12, 1))
                            .WithEnd(new DateOnly(2023, 12, 31))
                            .WithId(Guid.NewGuid())
                            .WithName("Period name")
                            .Build();

            var defaultPeriodResponse = _mapper.Map<DefaultPeriodResponse>(period);

            defaultPeriodResponse.Name.Should().Be(period.Name);
            defaultPeriodResponse.Id.Should().Be(period.Id);
        }

        // CreateMap<Period, ListPeriodsResponse>()
        [Fact]
        public void Period_To_ListPeriodsResponse()
        {
            var period = PeriodBuilder
                            .New
                            .WithStart(new DateOnly(2023, 12, 1))
                            .WithEnd(new DateOnly(2023, 12, 31))
                            .WithId(Guid.NewGuid())
                            .WithName("Period name")
                            .Build();

            var defaultPeriodResponse = _mapper.Map<ListPeriodsResponse>(period);

            defaultPeriodResponse.Name.Should().Be(period.Name);
            defaultPeriodResponse.Id.Should().Be(period.Id);
            defaultPeriodResponse.Start.Should().Be(period.Start);
            defaultPeriodResponse.End.Should().Be(period.End);
        }

        [Fact]
        public void Period_To_ListPeriodsResponse_ShouldSetIsCurrentToFalse_WhenStartAndEndAreNotToday()
        {
            var period = PeriodBuilder
                            .New
                            .WithStart(new DateOnly(2023, 11, 1))
                            .WithEnd(new DateOnly(2023, 11, 30))
                            .Build();

            var defaultPeriodResponse = _mapper.Map<ListPeriodsResponse>(period);

            defaultPeriodResponse.IsCurrent.Should().BeFalse();
        }

        [Fact]
        public void Period_To_ListPeriodsResponse_ShouldSetIsCurrentToTrue_WhenStartAndEndAreToday()
        {
            var period = PeriodBuilder
                            .New
                            .WithStart(DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-5))
                            .WithEnd(DateOnly.FromDateTime(DateTime.UtcNow).AddDays(5))
                            .Build();

            var defaultPeriodResponse = _mapper.Map<ListPeriodsResponse>(period);

            defaultPeriodResponse.IsCurrent.Should().BeTrue();
        }
    }
}
