namespace WebApi.Model;

public abstract class Register
{
    public Guid Id { get; set; }
    public DateTime Inserted { get; set; }
    public DateTime? Updated { get; set; }
}
