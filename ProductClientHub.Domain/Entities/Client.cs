namespace ProductClientHub.Domain.Entities;

public class Client
{
    public Guid Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public Client()
    {
        Id = Guid.NewGuid();
    }
}
