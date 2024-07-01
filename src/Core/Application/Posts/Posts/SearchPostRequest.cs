using csumathboy.Application.Posts.Posts;
using csumathboy.Application.Posts.Posts.Specifications;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Posts;

public class SearchPostRequest : PaginationFilter, IRequest<PaginationResponse<PostDto>>
{
    public string? Title { get; set; }

}

public class SearchPostRequestHandler : IRequestHandler<SearchPostRequest, PaginationResponse<PostDto>>
{
    private readonly IReadRepository<Post> _repository;

    public SearchPostRequestHandler(IReadRepository<Post> repository) => _repository = repository;

    public async Task<PaginationResponse<PostDto>> Handle(SearchPostRequest request, CancellationToken cancellationToken)
    {
        var spec = new PostSearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}