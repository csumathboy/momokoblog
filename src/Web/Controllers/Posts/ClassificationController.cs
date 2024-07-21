using csumathboy.Application.Posts.Classifications;
using Microsoft.AspNetCore.Mvc;

namespace csumathboy.Web.Controllers.Classifications;

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

}
