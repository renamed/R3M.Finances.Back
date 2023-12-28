using FluentAssertions;
using System.Linq.Expressions;
using WebApi.Extensions;

namespace UnitTests.Extensions;

public class PredicateExtensionsUnitTests
{
    [Fact]
    public void AndIf_ShouldFilter_WhenConditionIsTrue()
    {
        IEnumerable<int> lst = [1, 2, 3, 4];

        Expression<Func<int, bool>> predicate = (c) => 1 == 1;

        predicate = predicate.AndIf(x => x > 3, () => 2 == 2);
        lst = lst.Where(predicate.Compile());
                    
        lst.Should().HaveCount(1);
        lst.Should().Contain(4);
        lst.Should().NotContain(new[] {1,2,3 });
    }

    [Fact]
    public void AndIf_ShouldNotFilter_WhenConditionIsFalse()
    {
        IEnumerable<int> lst = [1, 2, 3, 4];

        Expression<Func<int, bool>> predicate = (c) => 1 == 1;

        predicate = predicate.AndIf(x => x > 3, () => 2 != 2);
        lst = lst.Where(predicate.Compile());

        lst.Should().HaveCount(4);
        lst.Should().Contain(new[] { 1, 2, 3, 4 });
    }

    [Fact]
    public void AndIf2_ShouldFilter_WhenConditionIsTrue()
    {
        IEnumerable<int> lst = [1, 2, 3, 4];

        Expression<Func<int, bool>> predicate = (c) => 1 == 1;

        predicate = predicate.AndIf(x => x > 3, true);
        lst = lst.Where(predicate.Compile());

        lst.Should().HaveCount(1);
        lst.Should().Contain(4);
        lst.Should().NotContain(new[] { 1, 2, 3 });
    }

    [Fact]
    public void AndIf2_ShouldNotFilter_WhenConditionIsFalse()
    {
        IEnumerable<int> lst = [1, 2, 3, 4];

        Expression<Func<int, bool>> predicate = (c) => 1 == 1;

        predicate = predicate.AndIf(x => x > 3, false);
        lst = lst.Where(predicate.Compile());

        lst.Should().HaveCount(4);
        lst.Should().Contain(new[] { 1, 2, 3, 4 });
    }
}
