using csumathboy.Application.Catalog.Brands;

namespace csumathboy.Application.Posts.Classifications;

public class ClassificationDetailsDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public int ArtCount { get; set; }
    public string? NickName { get; set; }
}