using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Posts.Specifications;

public class PostSearchRequestSpec : EntitiesByPaginationFilterSpec<Post, PostDto>
{
    public PostSearchRequestSpec(SearchPostRequest request)
        : base(request) =>
        Query
            .OrderBy(c => c.Sort, !request.HasOrderBy())
            .Where(p => p.ClassId.Equals(request.ClassId!), request.ClassId.HasValue)
            .Where(p => p.Sort >= request.MinimumSort!.Value, request.MinimumSort.HasValue)
            .Where(p => p.Sort <= request.MaximumSort!.Value, request.MaximumSort.HasValue);

}