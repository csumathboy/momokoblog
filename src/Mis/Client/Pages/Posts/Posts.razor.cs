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
    protected IBrandsClient BrandsClient { get; set; } = default!;

    protected EntityServerTableContext<PostDto, Guid, PostViewModel> Context { get; set; } = default!;

    private EntityTable<PostDto, Guid, PostViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["Post"],
            entityNamePlural: L["Post"],
            entityResource: FSHResource.Post,
            fields: new()
            {
                new(prod => prod.Id, L["Id"], "Id"),
                new(prod => prod.Name, L["Name"], "Name"),
                new(prod => prod.BrandName, L["Brand"], "Brand.Name"),
                new(prod => prod.Description, L["Description"], "Description"),
                new(prod => prod.Rate, L["Rate"], "Rate")
            },
            enableAdvancedSearch: true,
            idFunc: prod => prod.ClassId,
            searchFunc: async filter =>
            {
                var postFilter = filter.Adapt<SearchPostRequest>();

                postFilter.BrandId = SearchBrandId == default ? null : SearchBrandId;
                postFilter.MinimumRate = SearchMinimumRate;
                postFilter.MaximumRate = SearchMaximumRate;

                var result = await PostClient.SearchAsync(postFilter);
                return result.Adapt<PaginationResponse<PostDto>>();
            },
            createFunc: async prod =>
            {
                if (!string.IsNullOrEmpty(prod.ImageInBytes))
                {
                    prod.Image = new FileUploadRequest() { Data = prod.ImageInBytes, Extension = prod.ImageExtension ?? string.Empty, Name = $"{prod.Name}_{Guid.NewGuid():N}" };
                }

                await PostClient.CreateAsync(prod.Adapt<CreatePostRequest>());
                prod.ImageInBytes = string.Empty;
            },
            updateFunc: async (id, prod) =>
            {
                if (!string.IsNullOrEmpty(prod.ImageInBytes))
                {
                    prod.DeleteCurrentImage = true;
                    prod.Image = new FileUploadRequest() { Data = prod.ImageInBytes, Extension = prod.ImageExtension ?? string.Empty, Name = $"{prod.Name}_{Guid.NewGuid():N}" };
                }

                await PostClient.UpdateAsync(id, prod.Adapt<UpdatePostRequest>());
                prod.ImageInBytes = string.Empty;
            },
            exportFunc: async filter =>
            {
                var exportFilter = filter.Adapt<ExportPostRequest>();

                exportFilter.BrandId = SearchBrandId == default ? null : SearchBrandId;
                exportFilter.MinimumRate = SearchMinimumRate;
                exportFilter.MaximumRate = SearchMaximumRate;

                return await PostClient.ExportAsync(exportFilter);
            },
            deleteFunc: async id => await PostClient.DeleteAsync(id));

    // Advanced Search

    private Guid _searchBrandId;
    private Guid SearchBrandId
    {
        get => _searchBrandId;
        set
        {
            _searchBrandId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal _searchMinimumRate;
    private decimal SearchMinimumRate
    {
        get => _searchMinimumRate;
        set
        {
            _searchMinimumRate = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal _searchMaximumRate = 9999;
    private decimal SearchMaximumRate
    {
        get => _searchMaximumRate;
        set
        {
            _searchMaximumRate = value;
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