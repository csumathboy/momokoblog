using csumathboy.Application.Catalog.Brands;

namespace csumathboy.Application.Posts.Comments;

public class CommentDetailsDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid PostsId { get; set; }
    public string RealName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}