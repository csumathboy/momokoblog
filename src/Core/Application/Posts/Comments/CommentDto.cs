using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csumathboy.Application.Posts.Comments;
public class CommentDto : IDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public Guid PostsId { get; set; }
    public string RealName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
 }