using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Posts;

public class DeletePostRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeletePostRequest(Guid id) => Id = id;
}

public class DeletePostRequestHandler : IRequestHandler<DeletePostRequest, Guid>
{
    private readonly IRepository<Post> _repository;
    private readonly IStringLocalizer _t;

    public DeletePostRequestHandler(IRepository<Post> repository, IStringLocalizer<DeletePostRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(DeletePostRequest request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = post ?? throw new NotFoundException(_t["Post {0} Not Found."]);

        // Add Domain Events to be raised after the commit
        post.DomainEvents.Add(EntityDeletedEvent.WithEntity(post));

        await _repository.DeleteAsync(post, cancellationToken);

        return request.Id;
    }
}