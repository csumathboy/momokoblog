using csumathboy.Application.Posts.Comments;
using csumathboy.Application.Posts.Comments.Specifications;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Comments;

public class SearchCommentRequest : PaginationFilter, IRequest<PaginationResponse<CommentDto>>
{
    public string? Title { get; set; }

}

public class SearchCommentRequestHandler : IRequestHandler<SearchCommentRequest, PaginationResponse<CommentDto>>
{
    private readonly IReadRepository<Comment> _repository;

    public SearchCommentRequestHandler(IReadRepository<Comment> repository) => _repository = repository;

    public async Task<PaginationResponse<CommentDto>> Handle(SearchCommentRequest request, CancellationToken cancellationToken)
    {
        var spec = new CommentSearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}