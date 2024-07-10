using csumathboy.Client.Components.EntityTable;
using csumathboy.Client.Infrastructure.ApiClient;
using csumathboy.Client.Infrastructure.Common;
using csumathboy.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace csumathboy.Client.Pages.Posts;

public partial class Posts
{
    [Inject]
    protected IPostClient PostClient { get; set; } = default!;
    [Inject]
    protected IClassificationClient ClassificationClient { get; set; } = default!;

    protected EntityServerTableContext<PostDto, Guid, PostViewModel> Context { get; set; } = default!;

    private EntityTable<PostDto, Guid, PostViewModel> _table = default!;

    protected override void OnInitialized() =>
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
                return result.Adapt<PaginationResponse<PostDto>>();
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