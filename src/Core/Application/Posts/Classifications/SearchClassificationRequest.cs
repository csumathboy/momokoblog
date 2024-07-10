using csumathboy.Application.Posts.Classifications;
using csumathboy.Application.Posts.Classifications.Specifications;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Classifications;

public class SearchClassificationRequest : PaginationFilter, IRequest<PaginationResponse<ClassificationDto>>
{
}

public class SearchClassificationRequestHandler : IRequestHandler<SearchClassificationRequest, PaginationResponse<ClassificationDto>>
{
    private readonly IReadRepository<Classification> _repository;

    public SearchClassificationRequestHandler(IReadRepository<Classification> repository) => _repository = repository;

    public async Task<PaginationResponse<ClassificationDto>> Handle(SearchClassificationRequest request, CancellationToken cancellationToken)
    {
        var spec = new ClassificationSearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}