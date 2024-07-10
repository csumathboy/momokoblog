using csumathboy.Client.Components.EntityTable;
using csumathboy.Client.Infrastructure.ApiClient;
using csumathboy.Client.Infrastructure.Common;
using csumathboy.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace csumathboy.Client.Pages.Posts;
public partial class Tags
{
    [Inject]
    protected ITagClient TagsClient { get; set; } = default!;

    protected EntityServerTableContext<TagDto, Guid, TagViewModel> Context { get; set; } = default!;

    private EntityTable<TagDto, Guid, TagViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["Tag"],
            entityNamePlural: L["Tags"],
            entityResource: FSHResource.Tag,
            fields: new()
            {
                new(tagg => tagg.Id, L["Id"], "Id"),
                new(tagg => tagg.Name, L["Name"], "Name"),
                new(tagg => tagg.ArtCount, L["ArtCount"], "ArtCount"),
                new(tagg => tagg.NickName, L["NickName"], "NickName")
            },
            enableAdvancedSearch: false,
            idFunc: tagg => tagg.Id,
            searchFunc: async filter =>
            {
                var classificationFilter = filter.Adapt<SearchTagRequest>();
 
                var result = await TagsClient.SearchAsync(classificationFilter);
                return result.Adapt<PaginationResponse<TagDto>>();
            },
            createFunc: async tagg =>
            {
                await TagsClient.CreateAsync(tagg.Adapt<CreateTagRequest>());
            },
            updateFunc: async (id, tagg) =>
            {
                await TagsClient.UpdateAsync(id, tagg.Adapt<UpdateTagRequest>());
            },
            deleteFunc: async id => await TagsClient.DeleteAsync(id));

    // Advanced Search

}

public class TagViewModel : UpdateTagRequest
{
 
}