using csumathboy.Client.Components.EntityTable;
using csumathboy.Client.Infrastructure.ApiClient;
using csumathboy.Client.Infrastructure.Common;
using csumathboy.Client.Shared;
using csumathboy.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor;
using PSC.Blazor.Components.MarkdownEditor;
using System.Runtime.CompilerServices;
using static MudBlazor.CategoryTypes;

namespace csumathboy.Client.Pages.Posts;

public partial class Posts
{
    [Inject]
    private IAccessTokenProvider TokenProvider { get; set; } = default!;

    [Inject]
    protected IPostClient PostClient { get; set; } = default!;

    [Inject]
    private ITagClient TagClient { get; set; } = default!;

    [Inject]
    protected IClassificationClient ClassificationClient { get; set; } = default!;

    protected EntityServerTableContext<PostDto, Guid, PostViewModel> Context { get; set; } = default!;

    private EntityTable<PostDto, Guid, PostViewModel> _table = default!;
    protected override async Task OnInitializedAsync()
    {
        _uploadAuthetication = await TokenProvider.GetAccessTokenAsync();
        var filter = new SearchTagRequest
        {
            PageSize = 100
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => TagClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfTagDto response)
        {
            _tagList = response.Data.ToList();
        }

    }

    protected override void OnInitialized()
    {
        _localMarkdownImageTexts = new MarkdownImageTexts()
        {
            Init = "如需上传图片，请直接将图片拖拽到文本编辑框内，或从剪切板粘贴到编辑框。",
            OnDragEnter = "请放开拖拽的图片直接上传。",
            OnDrop = "上传图片....",
            Progress = "图片上传中...",
            OnUploaded = "图片已上传。",
            SizeUnits = "B, KB, MB"
        };

        Context = new(
            entityName: L["Post"],
            entityNamePlural: L["Post"],
            entityResource: FSHResource.Post,
            fields: new()
            {
                new(pos => pos.Id, L["Id"], "Id"),
                new(pos => pos.Title, L["Title"], "Title"),
                new(pos => pos.Classification.Name, L["Name"], "Classification.Name"),
                new(pos => pos.Author, L["Author"], "Author"),
                new(pos => pos.IsTop, L["IsTop"], "IsTop"),
                new(pos => pos.Sort, L["Sort"], "Sort")
            },
            enableAdvancedSearch: true,
            idFunc: pos => pos.Id,
            searchFunc: async filter =>
            {
                var postFilter = filter.Adapt<SearchPostRequest>();

                postFilter.ClassId = SearchClassId == default ? null : SearchClassId;
                postFilter.MinimumSort = SearchMinimumSort;
                postFilter.MaximumSort = SearchMaximumSort;

                var result = await PostClient.SearchAsync(postFilter);
                TypeAdapterConfig<PaginationResponseOfPostDto, PaginationResponse<PostDto>>.NewConfig()
                  .Fork(config => config.Default.PreserveReference(true));
                return result.Adapt<PaginationResponse<PostDto>>();
            },
            getDetailsFunc: async (id) =>
            {
                var postDetail = await PostClient.GetAsync(id);

                return new PostViewModel()
                {
                    Id = id,
                    Author = postDetail.Author,
                    ClassId = postDetail.ClassId,
                    DeleteCurrentImage = false,
                    ContextValue = postDetail.ContextValue,
                    Description = postDetail.Description,
                    IsTop = Convert.ToInt32(postDetail.IsTop),
                    Title = postDetail.Title,
                    Sort = postDetail.Sort,
                    TagList = string.Join(",", postDetail.Tags.Select(x => x.Name).ToList()),
                    PostsStatus = postDetail.PostsStatus.Value,
                    ImagePath = postDetail.Picture

                };
            },
            createFunc: async pos =>
            {
                if (!string.IsNullOrEmpty(pos.ImageInBytes))
                {
                    pos.Image = new FileUploadRequest() { Data = pos.ImageInBytes, Extension = pos.ImageExtension ?? string.Empty, Name = $"{pos.Title}_{Guid.NewGuid():N}" };
                }
                await PostClient.CreateAsync(pos.Adapt<CreatePostRequest>());
                pos.ImageInBytes = string.Empty;
            },
            updateFunc: async (id, pos) =>
            {
                if (!string.IsNullOrEmpty(pos.ImageInBytes))
                {
                    pos.DeleteCurrentImage = true;
                    pos.Image = new FileUploadRequest() { Data = pos.ImageInBytes, Extension = pos.ImageExtension ?? string.Empty, Name = $"{pos.Title}_{Guid.NewGuid():N}" };
                }

                await PostClient.UpdateAsync(id, pos.Adapt<UpdatePostRequest>());
                pos.ImageInBytes = string.Empty;
            },
            deleteFunc: async id => await PostClient.DeleteAsync(id));
    }

    // Advanced Search

    private Guid _searchClassId;
    private Guid SearchClassId
    {
        get => _searchClassId;
        set
        {
            _searchClassId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal _searchMinimumSort;
    private decimal SearchMinimumSort
    {
        get => _searchMinimumSort;
        set
        {
            _searchMinimumSort = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal _searchMaximumSort = 9999;
    private decimal SearchMaximumSort
    {
        get => _searchMaximumSort;
        set
        {
            _searchMaximumSort = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _uploadAuthetication;
    private string? UploadAuthetication
    {
        get => _uploadAuthetication;
        set
        {
            _uploadAuthetication = value;
        }
    }

    private MarkdownImageTexts? _localMarkdownImageTexts;
    private MarkdownImageTexts? LocalMarkdownImageTexts
    {
        get => _localMarkdownImageTexts;
        set
        {
            _localMarkdownImageTexts = value;
        }
    }

    private List<TagDto>? _tagList;
    private List<TagDto>? TagList
    {
        get => _tagList;
        set
        {
            _tagList = value;
        }
    }

    // TODO : Make this as a shared service or something? Since it's used by Profile Component also for now, and literally any other component that will have image upload.
    // The new service should ideally return $"data:{ApplicationConstants.StandardImageFormat};base64,{Convert.ToBase64String(buffer)}"
    private async Task UploadFiles(InputFileChangeEventArgs e)
    {
        if (e.File != null)
        {
            string? extension = Path.GetExtension(e.File.Name);
            if (!ApplicationConstants.SupportedImageFormats.Contains(extension.ToLower()))
            {
                Snackbar.Add("Image Format Not Supported.", Severity.Error);
                return;
            }

            Context.AddEditModal.RequestModel.ImageExtension = extension;
            var imageFile = await e.File.RequestImageFileAsync(ApplicationConstants.StandardImageFormat, ApplicationConstants.MaxImageWidth, ApplicationConstants.MaxImageHeight);
            byte[]? buffer = new byte[imageFile.Size];
            await imageFile.OpenReadStream(ApplicationConstants.MaxAllowedSize).ReadAsync(buffer);
            Context.AddEditModal.RequestModel.ImageInBytes = $"data:{ApplicationConstants.StandardImageFormat};base64,{Convert.ToBase64String(buffer)}";
            Context.AddEditModal.ForceRender();
        }
    }

    public void ClearImageInBytes()
    {
        Context.AddEditModal.RequestModel.ImageInBytes = string.Empty;
        Context.AddEditModal.ForceRender();
    }

    public void SetDeleteCurrentImageFlag()
    {
        Context.AddEditModal.RequestModel.ImageInBytes = string.Empty;
        Context.AddEditModal.RequestModel.ImagePath = string.Empty;
        Context.AddEditModal.RequestModel.DeleteCurrentImage = true;
        Context.AddEditModal.ForceRender();
    }
}

public class PostViewModel : UpdatePostRequest
{
    public string? ImagePath { get; set; }
    public string? ImageInBytes { get; set; }
    public string? ImageExtension { get; set; }

}