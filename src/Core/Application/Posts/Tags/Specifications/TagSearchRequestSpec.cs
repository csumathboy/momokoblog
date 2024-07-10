using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Tags.Specifications;

public class TagSearchRequestSpec : EntitiesByPaginationFilterSpec<Tag, TagDto>
{
    public TagSearchRequestSpec(SearchTagRequest request)
        : base(request) =>
        Query
            .OrderBy(c => c.Name, !request.HasOrderBy());
}