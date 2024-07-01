using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Tags;

public class DeleteTagRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteTagRequest(Guid id) => Id = id;
}

public class DeleteTagRequestHandler : IRequestHandler<DeleteTagRequest, Guid>
{
    private readonly IRepository<Tag> _repository;
    private readonly IStringLocalizer _t;

    public DeleteTagRequestHandler(IRepository<Tag> repository, IStringLocalizer<DeleteTagRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(DeleteTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = tag ?? throw new NotFoundException(_t["Tag {0} Not Found."]);

        // Add Domain Events to be raised after the commit
        tag.DomainEvents.Add(EntityDeletedEvent.WithEntity(tag));

        await _repository.DeleteAsync(tag, cancellationToken);

        return request.Id;
    }
}