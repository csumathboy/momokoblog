using Ardalis.SmartEnum;

namespace csumathboy.Domain.PostsAggregate;
public class PostStatus : SmartEnum<PostStatus>
{
    public static readonly PostStatus Pulish = new(nameof(Pulish), 1);
    public static readonly PostStatus Draft = new(nameof(Draft), 2);
    public static readonly PostStatus Delete = new(nameof(Delete), 3);

    protected PostStatus(string name, int value)
        : base(name, value)
    {
    }
}

