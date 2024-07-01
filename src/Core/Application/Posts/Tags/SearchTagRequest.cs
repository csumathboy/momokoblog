using csumathboy.Application.Posts.Tags;
using csumathboy.Application.Posts.Tags.Specifications;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Tags;

public class SearchTagRequest : PaginationFilter, IRequest<PaginationResponse<TagDto>>
{
    public string? Name { get; set; }

}

public class SearchTagRequestHandler : IRequestHandler<SearchTagRequest, PaginationResponse<TagDto>>
{
    private readonly IReadRepository<Tag> _repository;

    public SearchTagRequestHandler(IReadRepository<Tag> repository) => _repository = repository;

    public async Task<PaginationResponse<TagDto>> Handle(SearchTagRequest request, CancellationToken cancellationToken)
    {
        var spec = new TagSearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}