using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Comments.Specifications;

public class CommentSearchRequestSpec : EntitiesByPaginationFilterSpec<Comment, CommentDto>
{
    public CommentSearchRequestSpec(SearchCommentRequest request)
        : base(request) =>
        Query
            .OrderBy(c => c.Title, !request.HasOrderBy());
}