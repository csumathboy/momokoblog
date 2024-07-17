using csumathboy.Application.Posts.Posts.Specifications;
using csumathboy.Application.Posts.Tags;
using csumathboy.Application.Posts.Tags.Specifications;
using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;
using Mapster;
using System.Diagnostics;

namespace csumathboy.Application.Posts.Posts;

public class CreatePostRequest : IRequest<Guid>
{
    public string Title { get; set; } = default!;

    public string Author { get; set; } = default!;

    public string Description { get; set; } = string.Empty;

    public Guid ClassId { get; set; }

    public string ContextValue { get; set; } = default!;

    public int Sort { get; set; } = 0;

    public int IsTop { get; set; } = 0;

    public int PostsStatus { get; set; } = 1;

    public FileUploadRequest? Image { get; set; }

    public string TagList { get; set; } = string.Empty;
}

public class CreatePostRequestHandler : IRequestHandler<CreatePostRequest, Guid>
{
    private readonly IRepository<Post> _repository;
    private readonly IRepository<Classification> _classRepository;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IRepository<PostTag> _posttagRepository;
    private readonly IFileStorageService _file;
    public CreatePostRequestHandler(IRepository<Post> repository, IRepository<PostTag> posttagRepository, IRepository<Tag> tagRepository, IRepository<Classification> classRepository, IFileStorageService file) => (_repository, _posttagRepository, _tagRepository, _classRepository, _file) = (repository, posttagRepository, tagRepository, classRepository, file);

    public async Task<Guid> Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {
        string postImagePath = await _file.UploadAsync<Post>(request.Image, FileType.Image, cancellationToken);
        PostStatus postStatus = PostStatus.Pulish;
        switch (request.PostsStatus)
        {
            case 1:
                postStatus = PostStatus.Pulish; break;
            case 2:
                postStatus = PostStatus.Draft; break;
            case 3:
                postStatus = PostStatus.Delete; break;
            default:
                postStatus = PostStatus.Pulish; break;
        }

        var classfication = await _classRepository.GetByIdAsync(request.ClassId);

        var post = new Post(request.Title, request.ClassId, request.Author, request.Description, request.ContextValue, postImagePath, request.Sort, Convert.ToBoolean(request.IsTop), postStatus);
        post.UpdateClassification(classfication!);

        // Add Domain Events to be raised after the commit
        post.DomainEvents.Add(EntityCreatedEvent.WithEntity(post));

        post = await _repository.AddAsync(post, cancellationToken);

        // Add Tag
        var postTagList = new List<PostTag>();
        string[] tagList = request.TagList.Split(',');
        if (tagList != null && tagList.Length > 0)
        {
            foreach (string tagName in tagList)
            {
                string name = tagName.Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    var tag = await _tagRepository.FirstOrDefaultAsync(
                           (ISpecification<Tag>)new TagByNameSpec(name), cancellationToken);
                    if (tag != null)
                    {
                        postTagList.Add(new PostTag() { Post = post, Tag = tag });
                    }
                }
            }

            await _posttagRepository.AddRangeAsync(postTagList, cancellationToken);
        }

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