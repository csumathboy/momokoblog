using Ardalis.GuardClauses;
using csumathboy.Domain.Catalog;
using System.Collections.ObjectModel;
using System.Diagnostics;
namespace csumathboy.Domain.PostsAggregate;

public class PostTag : BaseEntity, IAggregateRoot
{
    public Guid PostId { get; set; }
    public Post Post { get; set; } = default!;

    public Guid TagId { get; set; }
    public string TagName { get; set; } = default!;

    public Tag Tag { get; set; } = default!;

}
