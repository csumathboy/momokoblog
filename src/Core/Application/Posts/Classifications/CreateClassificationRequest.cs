using csumathboy.Application.Posts.Classifications.Specifications;
using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;


namespace csumathboy.Application.Posts.Classifications;

public class CreateClassificationRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? NickName { get; set; }
}

public class CreateClassificationRequestHandler : IRequestHandler<CreateClassificationRequest, Guid>
{
    private readonly IRepository<Classification> _repository;
    public CreateClassificationRequestHandler(IRepository<Classification> repository) => _repository = repository;

    public async Task<Guid> Handle(CreateClassificationRequest request, CancellationToken cancellationToken)
    {

        var classification = new Classification(request.Name, request.NickName, request.Description);

        // Add Domain Events to be raised after the commit
        classification.DomainEvents.Add(EntityCreatedEvent.WithEntity(classification));

        await _repository.AddAsync(classification, cancellationToken);

        return classification.Id;
    }
}

public class CreateClassificationRequestValidator : CustomValidator<CreateClassificationRequest>
{
    public CreateClassificationRequestValidator(IReadRepository<Classification> classificationRepo, IStringLocalizer<CreateClassificationRequestValidator> T)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(1024)
            .MustAsync(async (name, ct) => await classificationRepo.FirstOrDefaultAsync(new ClassificationByNameSpec(name), ct) is null)
                .WithMessage((_, name) => T["Classification {0} already Exists.", name]);
    }
}