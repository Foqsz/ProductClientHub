namespace ProductClientHub.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMQ";

    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string QueueName { get; set; } = "clients.created";
    public string ExchangeName { get; set; } = "clients.exchange";
    public string ClientCreatedQueueName { get; set; } = "clients.created.audit";
    public string ClientWelcomeQueueName { get; set; } = "clients.created.welcome";
    public bool EnableConsumer { get; set; } = true;
}
