using csumathboy.Application.Posts.Tags.Specifications;
using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;


namespace csumathboy.Application.Posts.Tags;

public class CreateTagRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? NickName { get; set; }
}

public class CreateTagRequestHandler : IRequestHandler<CreateTagRequest, Guid>
{
    private readonly IRepository<Tag> _repository;
    public CreateTagRequestHandler(IRepository<Tag> repository) => _repository = repository;

    public async Task<Guid> Handle(CreateTagRequest request, CancellationToken cancellationToken)
    {

        var tag = new Tag(request.Name, request.NickName);

        // Add Domain Events to be raised after the commit
        tag.DomainEvents.Add(EntityCreatedEvent.WithEntity(tag));

        await _repository.AddAsync(tag, cancellationToken);

        return tag.Id;
    }
}

public class CreateTagRequestValidator : CustomValidator<CreateTagRequest>
{
    public CreateTagRequestValidator(IReadRepository<Tag> tagRepo, IStringLocalizer<CreateTagRequestValidator> T)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(1024)
            .MustAsync(async (name, ct) => await tagRepo.FirstOrDefaultAsync(new TagByNameSpec(name), ct) is null)
                .WithMessage((_, name) => T["Tag {0} already Exists.", name]);
    }
}