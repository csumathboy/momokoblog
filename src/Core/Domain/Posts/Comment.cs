using Ardalis.GuardClauses;

namespace csumathboy.Domain.PostsAggregate;
public class Comment : AuditableEntity, IAggregateRoot
{
    public string Title { get; set; }
    public Guid PostsId { get; set; }
    public string Description { get; set; }
    public string RealName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Comment(string title, Guid postsId, string description, string realName, string email, string phoneNumber)
    {
        Title = Guard.Against.NullOrEmpty(title, nameof(title));
        PostsId = Guard.Against.NullOrEmpty(postsId, nameof(postsId));
        Description = Guard.Against.NullOrEmpty(description, nameof(description));
        RealName = Guard.Against.NullOrEmpty(realName, nameof(realName));
        Email = Guard.Against.NullOrEmpty(email, nameof(email));
        PhoneNumber = Guard.Against.NullOrEmpty(phoneNumber, nameof(phoneNumber));
    }
}
