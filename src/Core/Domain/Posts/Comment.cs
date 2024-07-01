using Ardalis.GuardClauses;
using System.Xml.Linq;

namespace csumathboy.Domain.PostsAggregate;
public class Comment : AuditableEntity, IAggregateRoot
{
    public string Title { get; set; }
    public Guid PostsId { get; set; }
    public string Description { get; set; }
    public string RealName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Comment(string title, Guid postsId, string? description, string realName, string email, string phoneNumber)
    {
        Title = Guard.Against.NullOrEmpty(title, nameof(title));
        PostsId = Guard.Against.NullOrEmpty(postsId, nameof(postsId));
        Description = Guard.Against.NullOrEmpty(description, nameof(description));
        RealName = Guard.Against.NullOrEmpty(realName, nameof(realName));
        Email = Guard.Against.NullOrEmpty(email, nameof(email));
        PhoneNumber = Guard.Against.NullOrEmpty(phoneNumber, nameof(phoneNumber));
    }

    public void UpdateTitle(string newTitle)
    {
        Title = Guard.Against.NullOrEmpty(newTitle, nameof(newTitle));
    }

    public void UpdateRealName(string newRealName)
    {
        RealName = Guard.Against.NullOrEmpty(newRealName, nameof(newRealName));
    }
 
    public void UpdateDescription(string newDescription)
    {
        Description = Guard.Against.NullOrEmpty(newDescription, nameof(newDescription));
    }

    public void UpdateEmail(string newEmail)
    {
        Email = Guard.Against.NullOrEmpty(newEmail, nameof(newEmail));
    }

    public void UpdatePhoneNumber(string newPhoneNumber)
    {
        PhoneNumber = Guard.Against.NullOrEmpty(newPhoneNumber, nameof(newPhoneNumber));
    }


}
