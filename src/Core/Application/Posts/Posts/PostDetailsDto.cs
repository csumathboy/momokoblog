using csumathboy.Application.Catalog.Brands;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Posts;

public class PostDetailsDto : IDto
{
    public string Title { get; private set; } = default!;

    public string Author { get; set; } = default!;

    public string Description { get; set; } = string.Empty;

    public Classification Classification { get; set; } = default!;

    public Guid ClassId { get; private set; }

    public string ContextValue { get; set; } = default!;

    public string Picture { get; set; } = string.Empty;

    public int Sort { get; set; } = 0;

    public bool IsTop { get; set; } = false;

    public ICollection<Tag> Tags { get; private set; } = default!;

    public PostStatus PostsStatus { get; set; } = default!;
}