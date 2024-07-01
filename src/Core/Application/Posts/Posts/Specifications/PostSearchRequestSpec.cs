using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Posts.Specifications;

public class PostSearchRequestSpec : EntitiesByPaginationFilterSpec<Post, PostDto>
{
    public PostSearchRequestSpec(SearchPostRequest request)
        : base(request) =>
        Query
            .OrderBy(c => c.Title, !request.HasOrderBy())
            .Where(p => p.Title.Equals(request.Title));
}