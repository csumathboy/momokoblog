using csumathboy.Domain.PostsAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csumathboy.Application.Posts.Posts;
public class PostDto : IDto
{
    public string Title { get; private set; } = default!;

    public string Author { get; set; } = default!;

    public Classification Classification { get; set; } = default!;

    public Guid ClassId { get; private set; }

    public string Picture { get; set; } = string.Empty;

    public int Sort { get; set; } = 0;

    public bool IsTop { get; set; } = false;

    public PostStatus PostsStatus { get; set; } = default!;

    public ICollection<Tag> Tags { get;  set; } = default!;
}
