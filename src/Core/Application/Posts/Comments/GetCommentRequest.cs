using csumathboy.Application.Posts.Comments.Specifications;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Comments;

public class GetCommentRequest : IRequest<CommentDetailsDto>
{
    public Guid Id { get; set; }

    public GetCommentRequest(Guid id) => Id = id;
}

public class GetCommentRequestHandler : IRequestHandler<GetCommentRequest, CommentDetailsDto>
{
    private readonly IRepository<Comment> _repository;
    private readonly IStringLocalizer _t;

    public GetCommentRequestHandler(IRepository<Comment> repository, IStringLocalizer<GetCommentRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<CommentDetailsDto> Handle(GetCommentRequest request, CancellationToken cancellationToken) =>
        await _repository.FirstOrDefaultAsync(
            (ISpecification<Comment, CommentDetailsDto>)new CommentByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Comment {0} Not Found.", request.Id]);
}