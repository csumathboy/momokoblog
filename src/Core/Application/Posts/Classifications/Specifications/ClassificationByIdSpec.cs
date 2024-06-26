using csumathboy.Application.Posts.Classifications;
using csumathboy.Domain.PostsAggregate;

public class ClassificationByIdSpec : Specification<Classification, ClassificationDetailsDto>, ISingleResultSpecification
{
    public ClassificationByIdSpec(DefaultIdType id) =>
        Query
            .Where(p => p.Id == id);
}