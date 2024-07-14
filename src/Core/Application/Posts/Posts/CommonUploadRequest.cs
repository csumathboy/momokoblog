using csumathboy.Application.Posts.Posts.Specifications;
using csumathboy.Domain.Common.Events;
using csumathboy.Domain.PostsAggregate;

namespace csumathboy.Application.Posts.Posts;

public class CommonUploadRequest : IRequest<string>
{
    public FileUploadRequest? Image { get; set; }
}

public class CommonUploadRequestHandler : IRequestHandler<CommonUploadRequest, string>
{

    private readonly IFileStorageService _file;
    public CommonUploadRequestHandler(IFileStorageService file) => _file = file;

    public async Task<string> Handle(CommonUploadRequest request, CancellationToken cancellationToken)
    {
        string imagePath = await _file.UploadAsync<Post>(request.Image, FileType.Image, cancellationToken);
        return imagePath;
    }
}
