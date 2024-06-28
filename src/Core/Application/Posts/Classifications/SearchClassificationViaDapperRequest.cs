using csumathboy.Application.Common.Interfaces;
using csumathboy.Domain.PostsAggregate;
using Mapster;
using System.Collections.Generic;

namespace csumathboy.Application.Posts.Classifications;

public class SearchClassificationViaDapperRequest : IRequest<IReadOnlyList<ClassificationDto>>
{
    public string Name { get; set; }

    public SearchClassificationViaDapperRequest(string name) => Name = name;
}

public class SearchClassificationViaDapperRequestHandler : IRequestHandler<SearchClassificationViaDapperRequest, IReadOnlyList<ClassificationDto>>
{
    private readonly IDapperRepository _repository;
    private readonly IStringLocalizer _t;

    public SearchClassificationViaDapperRequestHandler(IDapperRepository repository, IStringLocalizer<GetClassificationViaDapperRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<IReadOnlyList<ClassificationDto>> Handle(SearchClassificationViaDapperRequest request, CancellationToken cancellationToken)
    {
        var classificationList = await _repository.QueryAsync<Classification>(
            $"SELECT * FROM Catalog.\"Classifications\" WHERE \"Name\"  like '%@Id%' AND \"TenantId\" = '@tenant'", new { Name= request.Name }, cancellationToken: cancellationToken);

        _ = classificationList ?? throw new NotFoundException(_t["Classifications {0} Not Found.", request.Name]);

        // Using mapster here throws a nullreference exception because of the "BrandName" property
        return classificationList.Adapt<IReadOnlyList<ClassificationDto>>();
    }
}