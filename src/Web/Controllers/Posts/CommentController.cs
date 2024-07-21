using csumathboy.Application.Posts.Comments;
using Microsoft.AspNetCore.Mvc;

namespace csumathboy.Web.Controllers.Comments;

public class CommentController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Comment)]
    [OpenApiOperation("Search Comments using available filters.", "")]
    public Task<PaginationResponse<CommentDto>> SearchAsync(SearchCommentRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Comment)]
    [OpenApiOperation("Get Comment details.", "")]
    public Task<CommentDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetCommentRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Comment)]
    [OpenApiOperation("Create a new Comment.", "")]
    public Task<Guid> CreateAsync(CreateCommentRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Comment)]
    [OpenApiOperation("Update a Comment.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateCommentRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Comment)]
    [OpenApiOperation("Delete a Comment.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteCommentRequest(id));
    }
}
