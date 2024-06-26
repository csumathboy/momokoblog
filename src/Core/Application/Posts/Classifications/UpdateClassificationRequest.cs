using csumathboy.Application.Posts.Classifications.Specifications;
using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Classifications;

public class UpdateClassificationRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? NickName { get; set; }

    public int ArtCount { get; set; }

}

public class UpdateClassificationRequestHandler : IRequestHandler<UpdateClassificationRequest, Guid>
{
    private readonly IRepository<Classification> _repository;
    private readonly IStringLocalizer _t;

    public UpdateClassificationRequestHandler(IRepository<Classification> repository, IStringLocalizer<UpdateClassificationRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(UpdateClassificationRequest request, CancellationToken cancellationToken)
    {
        var classification = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = classification ?? throw new NotFoundException(_t["Classification {0} Not Found.", request.Id]);

        if (!string.IsNullOrEmpty(request.Name))
        {
            classification.UpdateName(request.Name);
        }

        if(string.IsNullOrEmpty(request.Description))
        {
           classification.UpdateDescription(request.Description!);
        }

        if(string.IsNullOrEmpty(request.NickName))
        {
            classification.UpdateNickName(request.NickName!);
        }

        if(request.ArtCount > 0)
        {
            classification.UpdateArtCount(request.ArtCount);
        }

        // Add Domain Events to be raised after the commit
        classification.DomainEvents.Add(EntityUpdatedEvent.WithEntity(classification));

        await _repository.UpdateAsync(classification, cancellationToken);

        return request.Id;
    }
}

public class UpdateClassificationRequestValidator : CustomValidator<UpdateClassificationRequest>
{
    public UpdateClassificationRequestValidator(IReadRepository<Classification> ClassificationRepo, IReadRepository<Brand> brandRepo, IStringLocalizer<UpdateClassificationRequestValidator> T)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(1024)
            .MustAsync(async (Classification, name, ct) =>
                    await ClassificationRepo.FirstOrDefaultAsync(new ClassificationByNameSpec(name), ct)
                        is not Classification existingClassification || existingClassification.Id == Classification.Id)
                .WithMessage((_, name) => T["Classification {0} already Exists.", name]);
    }
}