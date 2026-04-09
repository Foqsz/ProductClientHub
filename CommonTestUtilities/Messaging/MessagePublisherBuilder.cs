using Moq;
using ProductClientHub.Domain.Services.Messaging;

namespace CommonTestUtilities.Messaging;

public static class MessagePublisherBuilder
{
    public static IMessagePublisher Build()
    {
        var mock = new Mock<IMessagePublisher>();

        mock.Setup(publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<It.IsAnyType>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return mock.Object;
    }
}
