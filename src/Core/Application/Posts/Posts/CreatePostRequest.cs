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

    public int Sort { get; set; } = 0;

    public bool IsTop { get; set; } = false;

    public PostStatus PostsStatus { get; set; } = default!;

    public FileUploadRequest? Image { get; set; }
}

public class CreatePostRequestHandler : IRequestHandler<CreatePostRequest, Guid>
{
    private readonly IRepository<Post> _repository;
    private readonly IFileStorageService _file;
    public CreatePostRequestHandler(IRepository<Post> repository, IFileStorageService file) => (_repository, _file) = (repository, file);

    public async Task<Guid> Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {
        string postImagePath = await _file.UploadAsync<Post>(request.Image, FileType.Image, cancellationToken);

        var post = new Post(request.Title, request.ClassId, request.Author, request.Description, request.ContextValue, postImagePath, request.Sort, request.IsTop, request.PostsStatus);

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