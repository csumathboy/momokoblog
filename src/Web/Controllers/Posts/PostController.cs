using csumathboy.Application.Posts.Posts;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;

namespace csumathboy.Web.Controllers.Posts;
public class PostController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Post)]
    [OpenApiOperation("Search posts using available filters.", "")]
    public Task<PaginationResponse<PostDto>> SearchAsync(SearchPostRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Post)]
    [OpenApiOperation("Get post details.", "")]
    public Task<PostDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetPostRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Post)]
    [OpenApiOperation("Create a new post.", "")]
    public Task<Guid> CreateAsync(CreatePostRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Post)]
    [OpenApiOperation("Update a post.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdatePostRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Post)]
    [OpenApiOperation("Delete a post.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeletePostRequest(id));
    }

    [HttpPost("upload")]
    [MustHavePermission(FSHAction.Update, FSHResource.Post)]
    [OpenApiOperation("Upload a file.", "")]
    public Task<string> UploadFile(CommonUploadRequest request)
    { 
        return Mediator.Send(request);
    }
}
