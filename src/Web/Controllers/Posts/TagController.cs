using csumathboy.Application.Posts.Tags;
using Microsoft.AspNetCore.Mvc;

namespace csumathboy.Web.Controllers.Tags;

public class TagController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Tag)]
    [OpenApiOperation("Search Tags using available filters.", "")]
    public Task<PaginationResponse<TagDto>> SearchAsync(SearchTagRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Tag)]
    [OpenApiOperation("Get Tag details.", "")]
    public Task<TagDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetTagRequest(id));
    }

    [HttpGet("{name}")]
    [MustHavePermission(FSHAction.View, FSHResource.Tag)]
    [OpenApiOperation("Get Tag details.", "")]
    public Task<TagDetailsDto> GetByNameAsync(string name)
    {
        return Mediator.Send(new GetByNameTagRequest(name));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Tag)]
    [OpenApiOperation("Create a new Tag.", "")]
    public Task<Guid> CreateAsync(CreateTagRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Tag)]
    [OpenApiOperation("Update a Tag.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateTagRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Tag)]
    [OpenApiOperation("Delete a Tag.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteTagRequest(id));
    }
}
