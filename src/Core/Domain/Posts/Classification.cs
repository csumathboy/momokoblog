
using Ardalis.GuardClauses;

namespace csumathboy.Domain.PostsAggregate;
public class Classification : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string NickName { get; set; }
    public int ArtCount { get; set; }

    public Classification(string name, string nickName, string description)
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        NickName = nickName;
        Description = description;
        ArtCount = 0;
    }

}
