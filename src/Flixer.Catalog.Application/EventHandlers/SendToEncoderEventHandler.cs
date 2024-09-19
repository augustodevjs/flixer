using Flixer.Catalog.Domain.Events;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Application.Intefaces;

namespace Flixer.Catalog.Application.EventHandlers;

public class SendToEncoderEventHandler : IDomainEventHandler<VideoUploadedEvent>
{
    private readonly IMessageProducer _messageProducer;

    public SendToEncoderEventHandler(IMessageProducer messageProducer)
        => _messageProducer = messageProducer;

    public Task HandleAsync(VideoUploadedEvent domainEvent)
        => _messageProducer.SendMessageAsync(domainEvent);
}