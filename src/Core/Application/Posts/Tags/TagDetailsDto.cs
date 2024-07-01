using csumathboy.Application.Catalog.Brands;

namespace csumathboy.Application.Posts.Tags;

public class TagDetailsDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int ArtCount { get; set; }
    public string? NickName { get; set; }
}