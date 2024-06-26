using csumathboy.Application.Posts.Classifications.Specifications;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Classifications;

public class GetClassificationRequest : IRequest<ClassificationDetailsDto>
{
    public Guid Id { get; set; }

    public GetClassificationRequest(Guid id) => Id = id;
}

public class GetClassificationRequestHandler : IRequestHandler<GetClassificationRequest, ClassificationDetailsDto>
{
    private readonly IRepository<Classification> _repository;
    private readonly IStringLocalizer _t;

    public GetClassificationRequestHandler(IRepository<Classification> repository, IStringLocalizer<GetClassificationRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<ClassificationDetailsDto> Handle(GetClassificationRequest request, CancellationToken cancellationToken) =>
        await _repository.FirstOrDefaultAsync(
            (ISpecification<Classification, ClassificationDetailsDto>)new ClassificationByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Classification {0} Not Found.", request.Id]);
}