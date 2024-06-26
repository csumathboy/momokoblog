using csumathboy.Domain.PostsAggregate;
using Mapster;

namespace csumathboy.Application.Posts.Classifications;

public class GetClassificationViaDapperRequest : IRequest<ClassificationDto>
{
    public Guid Id { get; set; }

    public GetClassificationViaDapperRequest(Guid id) => Id = id;
}

public class GetClassificationViaDapperRequestHandler : IRequestHandler<GetClassificationViaDapperRequest, ClassificationDto>
{
    private readonly IDapperRepository _repository;
    private readonly IStringLocalizer _t;

    public GetClassificationViaDapperRequestHandler(IDapperRepository repository, IStringLocalizer<GetClassificationViaDapperRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<ClassificationDto> Handle(GetClassificationViaDapperRequest request, CancellationToken cancellationToken)
    {
        var classification = await _repository.QueryFirstOrDefaultAsync<Classification>(
            $"SELECT * FROM Catalog.\"Classifications\" WHERE \"Id\"  = '{request.Id}' AND \"TenantId\" = '@tenant'", cancellationToken: cancellationToken);

        _ = classification ?? throw new NotFoundException(_t["Classification {0} Not Found.", request.Id]);

        // Using mapster here throws a nullreference exception because of the "BrandName" property
        // in ClassificationDto and the Classification not having a Brand assigned.
        return new ClassificationDto
        {
            Id = classification.Id,
            NickName = classification.NickName,
            Description = classification.Description,
            ArtCount = classification.ArtCount,
            Name = classification.Name
        };
    }
}