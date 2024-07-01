using Ardalis.GuardClauses;

namespace csumathboy.Domain.PostsAggregate;
public class Tag : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; }
    public string? NickName { get; set; }
    public int ArtCount { get; set; }

    public Tag(string name, string? nickName)
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        NickName = nickName;
        ArtCount = 0;
    }

    public void UpdateName(string newName)
    {
        Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
    }

    public void UpdateNickName(string newNickName)
    {
        NickName = Guard.Against.NullOrEmpty(newNickName, nameof(newNickName));
    }

    public void UpdateArtCount(int newArtCount)
    {
        ArtCount = Guard.Against.NegativeOrZero(newArtCount, nameof(newArtCount));
    }

}
