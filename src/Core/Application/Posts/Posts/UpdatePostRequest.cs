using csumathboy.Application.Posts.Posts.Specifications;
using csumathboy.Application.Posts.Tags;
using csumathboy.Application.Posts.Tags.Specifications;
using csumathboy.Domain.Catalog;
using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;
using Mapster;
using System.Diagnostics;
using System.Xml.Linq;

namespace csumathboy.Application.Posts.Posts;

public class UpdatePostRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;

    public string Author { get; set; } = default!;

    public string Description { get; set; } = string.Empty;

    public Guid ClassId { get; set; }

    public string ContextValue { get; set; } = default!;

    public int Sort { get; set; } = 0;

    public int IsTop { get; set; } = 0;

    public int PostsStatus { get; set; } = 0;

    public bool DeleteCurrentImage { get; set; } = false;

    public FileUploadRequest? Image { get; set; }

    public string TagList { get; set; } = string.Empty;


}

public class UpdatePostRequestHandler : IRequestHandler<UpdatePostRequest, Guid>
{
    private readonly IRepository<Post> _repository;
    private readonly IStringLocalizer _t;
    private readonly IFileStorageService _file;
    private readonly IRepository<Classification> _classRepository;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IRepository<PostTag> _posttagRepository;
    public UpdatePostRequestHandler(IRepository<Post> repository, IRepository<PostTag> posttagRepository, IRepository<Tag> tagRepository, IRepository<Classification> classRepository, IStringLocalizer<UpdatePostRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _tagRepository, _posttagRepository, _classRepository, _t, _file) = (repository, tagRepository, posttagRepository, classRepository, localizer, file);

    public async Task<Guid> Handle(UpdatePostRequest request, CancellationToken cancellationToken)
    {
        // var post = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var post = await _repository.FirstOrDefaultAsync(
                           (ISpecification<Post>)new PostByIdSpec(request.Id), cancellationToken);

        _ = post ?? throw new NotFoundException(_t["Post {0} Not Found.", request.Id]);

        // Remove old image if flag is set
        if (request.DeleteCurrentImage)
        {
            string? currentProductImagePath = post.Picture;
            if (!string.IsNullOrEmpty(currentProductImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                _file.Remove(Path.Combine(root, currentProductImagePath));
            }

            post = post.ClearImagePath();
        }

        string? productImagePath = request.Image is not null
            ? await _file.UploadAsync<Post>(request.Image, FileType.Image, cancellationToken)
            : null;

        if (!string.IsNullOrEmpty(request.Title))
        {
            post.UpdateTitle(request.Title);
        }

        if (!string.IsNullOrEmpty(request.Description))
        {
            post.UpdateDescription(request.Description);
        }

        if (!string.IsNullOrEmpty(request.Author))
        {
            post.UpdateAuthor(request.Author!);
        }

        if (!string.IsNullOrEmpty(request.ContextValue))
        {
            post.UpdateContextValue(request.ContextValue);
        }

        if (request.Sort > 0)
        {
            post.UpdateSort(request.Sort);
        }

        if (!string.IsNullOrEmpty(productImagePath))
        {
            post.UpdatePicture(productImagePath);
        }

        if (request.PostsStatus > 0)
        {
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

            post.UpdatePostsStatus(postStatus);
        }

        post.UpdateIsTop(Convert.ToBoolean(request.IsTop));

        if (request.ClassId != default)
        {
            post.UpdateClassification(request.ClassId);
        }

        var classfication = await _classRepository.GetByIdAsync(request.ClassId);
        post.UpdateClassification(classfication!);

        // Add Domain Events to be raised after the commit
        post.DomainEvents.Add(EntityUpdatedEvent.WithEntity(post));

        await _repository.UpdateAsync(post, cancellationToken);

        // Delete PostTag

        var deletePostTagList = new List<PostTag>();
        string requestTagList = request.TagList + ",";
        foreach (var postTag in post.PostTags)
        {
            if (!requestTagList.Contains(postTag.TagName + ","))
            {
                deletePostTagList.Add(postTag);
            }
        }

        if (deletePostTagList != null && deletePostTagList.Count > 0)
        {
            await _posttagRepository.DeleteRangeAsync(deletePostTagList, cancellationToken);
        }

        // Add New PostTag
        var postTagList = new List<PostTag>();
        string[] tagList = request.TagList.Split(',');
        if (tagList != null && tagList.Length > 0)
        {
            foreach (string tagName in tagList)
            {
                string name = tagName.Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    var exitPostTag = post.PostTags.Where(x => x.TagName.Equals(name)).FirstOrDefault();
                    if (exitPostTag == null)
                    {
                        var tag = await _tagRepository.FirstOrDefaultAsync(
                               (ISpecification<Tag>)new TagByNameSpec(name), cancellationToken);
                        if (tag != null)
                        {
                            postTagList.Add(new PostTag() { Post = post, Tag = tag, PostId = post.Id, TagId = tag.Id, TagName = tag.Name });
                        }
                    }

                }
            }

            if (postTagList != null && postTagList.Count > 0)
            {
                await _posttagRepository.AddRangeAsync(postTagList, cancellationToken);
            }
        }

        return request.Id;
    }

}

public class UpdatePostRequestValidator : CustomValidator<UpdatePostRequest>
{
    public UpdatePostRequestValidator(IReadRepository<Post> PostRepo, IReadRepository<Brand> brandRepo, IStringLocalizer<UpdatePostRequestValidator> T)
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(1024)
            .MustAsync(async (Post, title, ct) =>
                    await PostRepo.FirstOrDefaultAsync(new PostByTitleSpec(title), ct)
                        is not Post existingPost || existingPost.Id == Post.Id)
                .WithMessage((_, title) => T["Post {0} already Exists.", title]);
    }
}