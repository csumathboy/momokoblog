using csumathboy.Shared.Events;

namespace csumathboy.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}