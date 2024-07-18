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
    private readonly IRepository<Tag> _tagRepository;
    public GetPostRequestHandler(IRepository<Post> repository, IRepository<Tag> tagRepository, IStringLocalizer<GetPostRequestHandler> localizer) =>
        (_repository, _tagRepository, _t) = (repository, tagRepository, localizer);

    public async Task<PostDetailsDto> Handle(GetPostRequest request, CancellationToken cancellationToken)
    {
        var post = await _repository.FirstOrDefaultAsync(
             (ISpecification<Post, PostDetailsDto>)new PostByIdSpec(request.Id), cancellationToken)
         ?? throw new NotFoundException(_t["Post {0} Not Found.", request.Id]);

        // Get PostTagName

        string idList = string.Join(",", post.PostTags.Select(x => x.TagId.ToString()));
        var tagList = await _tagRepository.ListAsync(
             (ISpecification<Tag>)new TagByIdListSpec(idList), cancellationToken);
        if (tagList != null && tagList.Count > 0)
        {
            post.PostTagName = string.Join(",", tagList.Select(x => x.Name.ToString()));
        }

        return post;
    }
}