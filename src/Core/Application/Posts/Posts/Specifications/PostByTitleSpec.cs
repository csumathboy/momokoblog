using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Posts.Specifications;

public class PostByTitleSpec : Specification<Post>, ISingleResultSpecification
{
    public PostByTitleSpec(string title) =>
        Query.Where(p => p.Title == title);
}