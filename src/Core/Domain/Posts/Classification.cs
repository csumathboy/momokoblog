using Ardalis.GuardClauses;

namespace csumathboy.Domain.PostsAggregate;
public class Classification : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? NickName { get; set; }
    public int ArtCount { get; set; }

    public Classification()
    {
        Name = string.Empty;
    }

    public Classification(string name, string? nickName, string? description)
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        NickName = nickName;
        Description = description;
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

    public void UpdateDescription(string newDescription)
    {
        Description = Guard.Against.NullOrEmpty(newDescription, nameof(newDescription));
    }

    public void UpdateArtCount(int newArtCount)
    {
        ArtCount = Guard.Against.NegativeOrZero(newArtCount, nameof(newArtCount));
    }
}
