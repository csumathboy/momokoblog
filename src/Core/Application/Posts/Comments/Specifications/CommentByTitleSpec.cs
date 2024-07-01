using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Comments.Specifications;

public class CommentByTitleSpec : Specification<Comment>, ISingleResultSpecification
{
    public CommentByTitleSpec(string title) =>
        Query.Where(p => p.Title == title);
}