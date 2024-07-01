using csumathboy.Application.Posts.Posts.Specifications;
using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;


namespace csumathboy.Application.Posts.Posts;

public class CreatePostRequest : IRequest<Guid>
{
    public string Title { get; private set; } = default!;

    public string Author { get; set; } = default!;

    public string Description { get; set; } = string.Empty;

    public Guid ClassId { get; private set; }

    public string ContextValue { get; set; } = default!;

    public string Picture { get; set; } = string.Empty;

    public int Sort { get; set; } = 0;

    public bool IsTop { get; set; } = false;

    public PostStatus PostsStatus { get; set; } = default!;
}

public class CreatePostRequestHandler : IRequestHandler<CreatePostRequest, Guid>
{
    private readonly IRepository<Post> _repository;
    public CreatePostRequestHandler(IRepository<Post> repository) => _repository = repository;

    public async Task<Guid> Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {

        var post = new Post(request.Title, request.ClassId, request.Author, request.Description, request.ContextValue, request.Picture, request.Sort, request.IsTop, request.PostsStatus);

        // Add Domain Events to be raised after the commit
        post.DomainEvents.Add(EntityCreatedEvent.WithEntity(post));

        await _repository.AddAsync(post, cancellationToken);

        return post.Id;
    }
}

public class CreatePostRequestValidator : CustomValidator<CreatePostRequest>
{
    public CreatePostRequestValidator(IReadRepository<Post> postRepo, IStringLocalizer<CreatePostRequestValidator> T)
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(1024)
            .MustAsync(async (title, ct) => await postRepo.FirstOrDefaultAsync(new PostByTitleSpec(title), ct) is null)
                .WithMessage((_, title) => T["Post {0} already Exists.", title]);
    }
}