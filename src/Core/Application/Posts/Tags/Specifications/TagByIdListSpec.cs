using csumathboy.Application.Posts.Tags;
using csumathboy.Domain.PostsAggregate;

public class TagByIdListSpec : Specification<Tag>, ISingleResultSpecification
{
    public TagByIdListSpec(string idList) =>
        Query
            .Where(p => idList.Contains(p.Id.ToString()));
}