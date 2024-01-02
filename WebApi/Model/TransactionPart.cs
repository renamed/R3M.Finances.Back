namespace WebApi.Model
{
    public class TransactionPart : Register
    {
        public string Description { get; set; }
        public decimal Value { get; set; }
        public Category Category { get; set; }
        public Guid? CategoryId { get; set; }
        public Transaction Transaction { get; set; }
        public Guid TransactionId { get; set; }
    }
}
