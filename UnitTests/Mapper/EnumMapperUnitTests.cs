using AutoMapper;
using FluentAssertions;
using WebApi.Model;

namespace UnitTests.Mapper
{
    public class EnumMapperUnitTests
    {       
        private readonly IMapper _mapper;

        public EnumMapperUnitTests()
        {
            _mapper = MapperFactory.GetEnumMapper();
        }

        [Theory]
        [InlineData(TransactionType.Unknown, TransactionType.Unknown)]
        [InlineData(TransactionType.Credit, TransactionType.Credit)]
        [InlineData(TransactionType.Debit, TransactionType.Debit)]
        [InlineData(null, TransactionType.Unknown)]
        public void TransactionTypeNullable_To_TransactionType(TransactionType? origin,  TransactionType destiny)
        {
            var response = _mapper.Map<TransactionType>(origin);
            response.Should().Be(destiny);
        }
    }
}
