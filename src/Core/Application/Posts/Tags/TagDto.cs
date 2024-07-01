using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csumathboy.Application.Posts.Tags;
public class TagDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? NickName { get; set; }
    public int ArtCount { get; set; }
}
