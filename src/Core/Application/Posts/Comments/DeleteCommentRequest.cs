using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Comments;

public class DeleteCommentRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteCommentRequest(Guid id) => Id = id;
}

public class DeleteCommentRequestHandler : IRequestHandler<DeleteCommentRequest, Guid>
{
    private readonly IRepository<Comment> _repository;
    private readonly IStringLocalizer _t;

    public DeleteCommentRequestHandler(IRepository<Comment> repository, IStringLocalizer<DeleteCommentRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(DeleteCommentRequest request, CancellationToken cancellationToken)
    {
        var Comment = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = Comment ?? throw new NotFoundException(_t["Comment {0} Not Found."]);

        // Add Domain Events to be raised after the commit
        Comment.DomainEvents.Add(EntityDeletedEvent.WithEntity(Comment));

        await _repository.DeleteAsync(Comment, cancellationToken);

        return request.Id;
    }
}