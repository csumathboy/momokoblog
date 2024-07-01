using csumathboy.Application.Posts.Tags.Specifications;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Tags;

public class GetTagRequest : IRequest<TagDetailsDto>
{
    public Guid Id { get; set; }

    public GetTagRequest(Guid id) => Id = id;
}

public class GetTagRequestHandler : IRequestHandler<GetTagRequest, TagDetailsDto>
{
    private readonly IRepository<Tag> _repository;
    private readonly IStringLocalizer _t;

    public GetTagRequestHandler(IRepository<Tag> repository, IStringLocalizer<GetTagRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<TagDetailsDto> Handle(GetTagRequest request, CancellationToken cancellationToken) =>
        await _repository.FirstOrDefaultAsync(
            (ISpecification<Tag, TagDetailsDto>)new TagByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Tag {0} Not Found.", request.Id]);
}