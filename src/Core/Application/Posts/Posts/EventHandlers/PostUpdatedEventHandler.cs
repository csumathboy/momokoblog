using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.EventHandlers;

public class PostUpdatedEventHandler : EventNotificationHandler<EntityUpdatedEvent<Post>>
{
    private readonly ILogger<PostUpdatedEventHandler> _logger;

    public PostUpdatedEventHandler(ILogger<PostUpdatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityUpdatedEvent<Post> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}