using Ardalis.GuardClauses;
using System.Collections.ObjectModel;
using System.Diagnostics;
namespace csumathboy.Domain.PostsAggregate;
public class Post : AuditableEntity, IAggregateRoot
{
    public string Title { get; private set; }

    public string Author { get; set; }

    public string Description { get; set; } = string.Empty;

    public Classification Classification { get; set; } = new(string.Empty, string.Empty, string.Empty);

    public Guid ClassId { get; private set; }

    public string ContextValue { get; set; }

    public string Picture { get; set; } = string.Empty;

    public int Sort { get; set; } = 0;

    public bool IsTop { get; set; } = false;

    public ICollection<Tag> Tags { get; private set; }

    public PostStatus PostsStatus { get; set; }

    public Post(string title, Guid classId, string author, string description, string contextValue, string picture, int sort, bool isTop, PostStatus postsStatus)
    {
        Title = Guard.Against.NullOrEmpty(title, nameof(title));
        ClassId = Guard.Against.NullOrEmpty(classId);
        Author = Guard.Against.NullOrEmpty(author);
        Description = description;
        ContextValue = contextValue;
        Picture = picture;
        Sort = sort;
        IsTop = isTop;
        PostsStatus = postsStatus;
        Tags = new Collection<Tag>();
    }

    public void AddTags(Tag newTags)
    {
        Guard.Against.Null(newTags, nameof(newTags));
        Tags.Add(newTags);

    }

    public void UpdateTitle(string newTitle)
    {
        Title = Guard.Against.NullOrEmpty(newTitle, nameof(newTitle));
    }

    public void UpdateClassification(Guid newClassId)
    {
        ClassId = Guard.Against.NullOrEmpty(newClassId);
    }

    public void UpdateAuthor(string newAuthor)
    {
        Author = Guard.Against.NullOrEmpty(newAuthor, newAuthor);
    }

    public void UpdateDescription(string newDescription)
    {
        Description = Guard.Against.NullOrEmpty(newDescription, nameof(newDescription));
    }

    public void UpdateContextValue(string newContextValue)
    {
        ContextValue = Guard.Against.NullOrEmpty(newContextValue, nameof(newContextValue));
    }

    public void UpdatePicture(string newPicture)
    {
        Picture = Guard.Against.NullOrEmpty(newPicture, nameof(newPicture));
    }

    public void UpdateSort(int newSort)
    {
        Sort = Guard.Against.NegativeOrZero(newSort, nameof(newSort));
    }

    public void UpdatePostsStatus(PostStatus newPostsStatus)
    {
        PostsStatus = newPostsStatus;
    }
}
