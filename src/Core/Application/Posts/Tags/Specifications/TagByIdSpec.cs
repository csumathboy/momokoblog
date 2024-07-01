using csumathboy.Application.Posts.Tags;
using csumathboy.Domain.PostsAggregate;

public class TagByIdSpec : Specification<Tag, TagDetailsDto>, ISingleResultSpecification
{
    public TagByIdSpec(DefaultIdType id) =>
        Query
            .Where(p => p.Id == id);
}