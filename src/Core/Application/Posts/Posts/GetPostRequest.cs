using csumathboy.Application.Posts.Posts.Specifications;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Posts;

public class GetPostRequest : IRequest<PostDetailsDto>
{
    public Guid Id { get; set; }

    public GetPostRequest(Guid id) => Id = id;
}

public class GetPostRequestHandler : IRequestHandler<GetPostRequest, PostDetailsDto>
{
    private readonly IRepository<Post> _repository;
    private readonly IStringLocalizer _t;

    public GetPostRequestHandler(IRepository<Post> repository, IStringLocalizer<GetPostRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<PostDetailsDto> Handle(GetPostRequest request, CancellationToken cancellationToken) =>
        await _repository.FirstOrDefaultAsync(
            (ISpecification<Post, PostDetailsDto>)new PostByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Post {0} Not Found.", request.Id]);
}