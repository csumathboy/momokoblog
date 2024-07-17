using csumathboy.Application.Posts.Posts;
using csumathboy.Domain.PostsAggregate;

public class PostByIdSpec : Specification<Post, PostDetailsDto>, ISingleResultSpecification
{
    public PostByIdSpec(DefaultIdType id) =>
        Query.Include(p => p.PostTags)
            .Where(p => p.Id == id);
}