using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Classifications.Specifications;

public class ClassificationSearchRequestSpec : EntitiesByPaginationFilterSpec<Classification, ClassificationDto>
{
    public ClassificationSearchRequestSpec(SearchClassificationRequest request)
        : base(request) =>
        Query
            .OrderBy(c => c.Name, !request.HasOrderBy())
            .Where(p => p.Name.Equals(request.Name));
}