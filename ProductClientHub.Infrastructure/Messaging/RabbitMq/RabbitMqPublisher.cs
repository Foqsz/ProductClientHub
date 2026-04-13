using Microsoft.Extensions.Options;
using ProductClientHub.Domain.Services.Messaging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProductClientHub.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqPublisher : IMessagePublisher
{
    private readonly RabbitMqOptions _options;

    public RabbitMqPublisher(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
    }

    public async Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password
        };

        await using var connection = await factory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var payload = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(payload);

        var properties = new BasicProperties { Persistent = true };

        await channel.BasicPublishAsync(
            exchange: _options.ExchangeName,
            routingKey: queueName,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }
}
