using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.EventHandlers;

public class PostDeletedEventHandler : EventNotificationHandler<EntityDeletedEvent<Post>>
{
    private readonly ILogger<PostDeletedEventHandler> _logger;

    public PostDeletedEventHandler(ILogger<PostDeletedEventHandler> logger) => _logger = logger;

    public override Task Handle(EntityDeletedEvent<Post> @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", @event.GetType().Name);
        return Task.CompletedTask;
    }
}