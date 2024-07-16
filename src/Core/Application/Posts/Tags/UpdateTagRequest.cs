using csumathboy.Application.Posts.Tags.Specifications;
using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Tags;

public class UpdateTagRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? NickName { get; set; }

    public int ArtCount { get; set; }

}

public class UpdateTagRequestHandler : IRequestHandler<UpdateTagRequest, Guid>
{
    private readonly IRepository<Tag> _repository;
    private readonly IStringLocalizer _t;

    public UpdateTagRequestHandler(IRepository<Tag> repository, IStringLocalizer<UpdateTagRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(UpdateTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = tag ?? throw new NotFoundException(_t["Tag {0} Not Found.", request.Id]);

        if (!string.IsNullOrEmpty(request.Name))
        {
            tag.UpdateName(request.Name);
        }

        if(string.IsNullOrEmpty(request.NickName))
        {
            tag.UpdateNickName(request.NickName!);
        }

        if(request.ArtCount > 0)
        {
            tag.UpdateArtCount(request.ArtCount);
        }

        // Add Domain Events to be raised after the commit
        tag.DomainEvents.Add(EntityUpdatedEvent.WithEntity(tag));

        await _repository.UpdateAsync(tag, cancellationToken);

        return request.Id;
    }
}

public class UpdateTagRequestValidator : CustomValidator<UpdateTagRequest>
{
    public UpdateTagRequestValidator(IReadRepository<Tag> TagRepo, IStringLocalizer<UpdateTagRequestValidator> T)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(1024)
            .MustAsync(async (Tag, name, ct) =>
                    await TagRepo.FirstOrDefaultAsync(new TagByNameSpec(name), ct)
                        is not TagDetailsDto existingTag || existingTag.Id == Tag.Id)
                .WithMessage((_, name) => T["Tag {0} already Exists.", name]);
    }
}