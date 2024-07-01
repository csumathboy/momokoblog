using csumathboy.Application.Posts.Comments;
using csumathboy.Domain.PostsAggregate;

public class CommentByIdSpec : Specification<Comment, CommentDetailsDto>, ISingleResultSpecification
{
    public CommentByIdSpec(DefaultIdType id) =>
        Query
            .Where(p => p.Id == id);
}