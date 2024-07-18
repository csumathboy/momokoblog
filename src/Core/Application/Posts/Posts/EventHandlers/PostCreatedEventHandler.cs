using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.EventHandlers;

public class PostCreatedEventHandler : EventNotificationHandler<EntityCreatedEvent<Post>>
{
    private readonly ILogger<PostCreatedEventHandler> _logger;

    public PostCreatedEventHandler(ILogger<PostCreatedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityCreatedEvent<Post> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}