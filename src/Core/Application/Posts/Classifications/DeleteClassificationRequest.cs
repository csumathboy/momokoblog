using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Classifications;

public class DeleteClassificationRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteClassificationRequest(Guid id) => Id = id;
}

public class DeleteClassificationRequestHandler : IRequestHandler<DeleteClassificationRequest, Guid>
{
    private readonly IRepository<Classification> _repository;
    private readonly IStringLocalizer _t;

    public DeleteClassificationRequestHandler(IRepository<Classification> repository, IStringLocalizer<DeleteClassificationRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(DeleteClassificationRequest request, CancellationToken cancellationToken)
    {
        var classification = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = classification ?? throw new NotFoundException(_t["Classification {0} Not Found."]);

        // Add Domain Events to be raised after the commit
        classification.DomainEvents.Add(EntityDeletedEvent.WithEntity(classification));

        await _repository.DeleteAsync(classification, cancellationToken);

        return request.Id;
    }
}