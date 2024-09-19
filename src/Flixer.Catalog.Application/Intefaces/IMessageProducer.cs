namespace Flixer.Catalog.Application.Intefaces;

public interface IMessageProducer
{
    Task SendMessageAsync<T>(T message);
}