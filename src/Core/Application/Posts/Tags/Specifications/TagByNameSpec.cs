using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Tags.Specifications;

public class TagByNameSpec : Specification<Tag>, ISingleResultSpecification
{
    public TagByNameSpec(string name) =>
        Query.Where(p => p.Name == name);
}