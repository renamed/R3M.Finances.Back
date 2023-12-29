namespace WebApi.Model.Builders
{
    public class PeriodBuilder
    {
        private readonly Period _period;

        public static PeriodBuilder New => new();

        public PeriodBuilder WithStart(DateOnly start)
        {
            _period.Start = start;
            return this;
        }
        public PeriodBuilder WithEnd(DateOnly end)
        {
            _period.End = end;
            return this;
        }
        public PeriodBuilder WithName(string name)
        {
            _period.Name = name;
            return this;
        }
        public PeriodBuilder WithId(Guid id)
        {
            _period.Id = id;
            return this;
        }
        public Period Build()
        {
            return _period;
        }
    }
}
