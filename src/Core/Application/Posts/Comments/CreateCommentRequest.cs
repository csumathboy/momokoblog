using csumathboy.Application.Posts.Comments.Specifications;
using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Comments;

public class CreateCommentRequest : IRequest<Guid>
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public Guid PostsId { get; set; }
    public string RealName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}

public class CreateCommentRequestHandler : IRequestHandler<CreateCommentRequest, Guid>
{
    private readonly IRepository<Comment> _repository;
    public CreateCommentRequestHandler(IRepository<Comment> repository) => _repository = repository;

    public async Task<Guid> Handle(CreateCommentRequest request, CancellationToken cancellationToken)
    {

        var comment = new Comment(request.Title, request.PostsId, request.Description,request.RealName,request.Email,request.PhoneNumber);

        // Add Domain Events to be raised after the commit
        comment.DomainEvents.Add(EntityCreatedEvent.WithEntity(comment));

        await _repository.AddAsync(comment, cancellationToken);

        return comment.Id;
    }
}

public class CreateCommentRequestValidator : CustomValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator(IReadRepository<Comment> CommentRepo, IStringLocalizer<CreateCommentRequestValidator> T)
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .MaximumLength(1024)
            .MustAsync(async (name, ct) => await CommentRepo.FirstOrDefaultAsync(new CommentByTitleSpec(name), ct) is null)
                .WithMessage((_, name) => T["Comment {0} already Exists.", name]);
    }
}