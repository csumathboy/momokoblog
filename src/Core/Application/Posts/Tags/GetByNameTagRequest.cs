using csumathboy.Application.Posts.Tags.Specifications;
using csumathboy.Domain.PostsAggregate;
using System.Xml.Linq;

namespace csumathboy.Application.Posts.Tags;

public class GetByNameTagRequest : IRequest<TagDetailsDto>
{
    public string Name { get; set; }

    public GetByNameTagRequest(string name) => Name = name;
}

public class GetByNameTagRequestHandler : IRequestHandler<GetByNameTagRequest, TagDetailsDto>
{
    private readonly IRepository<Tag> _repository;
    private readonly IStringLocalizer _t;

    public GetByNameTagRequestHandler(IRepository<Tag> repository, IStringLocalizer<GetTagRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<TagDetailsDto> Handle(GetByNameTagRequest request, CancellationToken cancellationToken) =>
        await _repository.FirstOrDefaultAsync(
            (ISpecification<Tag, TagDetailsDto>)new TagByNameSpec(request.Name), cancellationToken)
        ?? throw new NotFoundException(_t["Tag {0} Not Found.", request.Name]);
}