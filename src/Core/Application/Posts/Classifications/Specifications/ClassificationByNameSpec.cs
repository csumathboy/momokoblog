using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Classifications.Specifications;

public class ClassificationByNameSpec : Specification<Classification>, ISingleResultSpecification
{
    public ClassificationByNameSpec(string name) =>
        Query.Where(p => p.Name == name);
}