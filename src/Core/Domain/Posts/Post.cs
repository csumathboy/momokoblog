using Ardalis.GuardClauses;
using csumathboy.Domain.Catalog;
using System.Collections.ObjectModel;
using System.Diagnostics;
namespace csumathboy.Domain.PostsAggregate;
public class Post : AuditableEntity, IAggregateRoot
{
    public string Title { get; set; }

    public string Author { get; set; }

    public string Description { get; set; } = string.Empty;

    public Classification? Classification { get; set; }

    public Guid ClassId { get; set; }

    public string ContextValue { get; set; }

    public string Picture { get; set; } = string.Empty;

    public int Sort { get; set; } = 0;

    public bool IsTop { get; set; } = false;

    public ICollection<Tag>? Tags { get; set; }

    public PostStatus PostsStatus { get; set; }

    public Post(string title, Guid classId, string author, string description, string contextValue, string picture, int sort, bool isTop, PostStatus postsStatus)
    {
        Title = Guard.Against.NullOrEmpty(title, nameof(title));
        ClassId = classId;
        Author = Guard.Against.NullOrEmpty(author, nameof(author));
        Description = description;
        ContextValue = contextValue;
        Picture = picture;
        Sort = sort;
        IsTop = isTop;
        PostsStatus = postsStatus;
        Tags = new Collection<Tag>();
    }

    public Post ClearImagePath()
    {
        Picture = string.Empty;
        return this;
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
        ClassId = newClassId;
    }

    public void UpdateClassification(Classification newClassification)
    {
        Classification = newClassification;
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

    public void UpdateIsTop(bool newIsTop)
    {
        IsTop = newIsTop;
    }

    public void UpdatePostsStatus(PostStatus newPostsStatus)
    {
        PostsStatus = newPostsStatus;
    }
}
