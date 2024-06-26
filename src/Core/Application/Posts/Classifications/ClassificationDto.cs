using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csumathboy.Application.Posts.Classifications;
public class ClassificationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? NickName { get; set; }
    public int ArtCount { get; set; }
}
