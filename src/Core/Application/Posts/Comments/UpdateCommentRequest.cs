using csumathboy.Application.Posts.Comments.Specifications;
using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Comments;

public class UpdateCommentRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string RealName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

}

public class UpdateCommentRequestHandler : IRequestHandler<UpdateCommentRequest, Guid>
{
    private readonly IRepository<Comment> _repository;
    private readonly IStringLocalizer _t;

    public UpdateCommentRequestHandler(IRepository<Comment> repository, IStringLocalizer<UpdateCommentRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(UpdateCommentRequest request, CancellationToken cancellationToken)
    {
        var comment = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = comment ?? throw new NotFoundException(_t["Comment {0} Not Found.", request.Id]);

        if (!string.IsNullOrEmpty(request.Title))
        {
            comment.UpdateTitle(request.Title);
        }

        if(string.IsNullOrEmpty(request.Description))
        {
            comment.UpdateDescription(request.Description!);
        }

        if(string.IsNullOrEmpty(request.RealName))
        {
            comment.UpdateRealName(request.RealName!);
        }

        if (string.IsNullOrEmpty(request.Email))
        {
            comment.UpdateEmail(request.Email!);
        }

        if (string.IsNullOrEmpty(request.PhoneNumber))
        {
            comment.UpdatePhoneNumber(request.PhoneNumber!);
        }

        // Add Domain Events to be raised after the commit
        comment.DomainEvents.Add(EntityUpdatedEvent.WithEntity(comment));

        await _repository.UpdateAsync(comment, cancellationToken);

        return request.Id;
    }
}

public class UpdateCommentRequestValidator : CustomValidator<UpdateCommentRequest>
{
    public UpdateCommentRequestValidator(IReadRepository<Comment> CommentRepo, IReadRepository<Brand> brandRepo, IStringLocalizer<UpdateCommentRequestValidator> T)
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(1024)
            .MustAsync(async (Comment, title, ct) =>
                    await CommentRepo.FirstOrDefaultAsync(new CommentByTitleSpec(title), ct)
                        is not Comment existingComment || existingComment.Id == Comment.Id)
                .WithMessage((_, title) => T["Comment {0} already Exists.", title]);
    }
}