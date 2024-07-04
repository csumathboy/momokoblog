using csumathboy.Application.Posts.Classifications;
using Microsoft.AspNetCore.Mvc;

namespace csumathboy.MisApi.Controllers.Classifications;

public class ClassificationController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Classification)]
    [OpenApiOperation("Search classifications using available filters.", "")]
    public Task<PaginationResponse<ClassificationDto>> SearchAsync(SearchClassificationRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Classification)]
    [OpenApiOperation("Get classification details.", "")]
    public Task<ClassificationDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetClassificationRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Classification)]
    [OpenApiOperation("Create a new classification.", "")]
    public Task<Guid> CreateAsync(CreateClassificationRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Classification)]
    [OpenApiOperation("Update a classification.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateClassificationRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Classification)]
    [OpenApiOperation("Delete a classification.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteClassificationRequest(id));
    }
}
