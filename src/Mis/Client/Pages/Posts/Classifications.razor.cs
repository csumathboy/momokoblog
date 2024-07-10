using csumathboy.Client.Components.EntityTable;
using csumathboy.Client.Infrastructure.ApiClient;
using csumathboy.Client.Infrastructure.Common;
using csumathboy.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace csumathboy.Client.Pages.Posts;
public partial class Classifications
{
    [Inject]
    protected IClassificationClient ClassificationsClient { get; set; } = default!;

    protected EntityServerTableContext<ClassificationDto, Guid, ClassificationViewModel> Context { get; set; } = default!;

    private EntityTable<ClassificationDto, Guid, ClassificationViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["Classification"],
            entityNamePlural: L["Classifications"],
            entityResource: FSHResource.Classification,
            fields: new()
            {
                new(classifiy => classifiy.Id, L["Id"], "Id"),
                new(classifiy => classifiy.Name, L["Name"], "Name"),
                new(classifiy => classifiy.Description, L["Description"], "Description"),
                new(classifiy => classifiy.ArtCount, L["ArtCount"], "ArtCount"),
                new(classifiy => classifiy.NickName, L["NickName"], "NickName")
            },
            enableAdvancedSearch: false,
            idFunc: classifiy => classifiy.Id,
            searchFunc: async filter =>
            {
                var classificationFilter = filter.Adapt<SearchClassificationRequest>();
 
                var result = await ClassificationsClient.SearchAsync(classificationFilter);
                return result.Adapt<PaginationResponse<ClassificationDto>>();
            },
            createFunc: async classifiy =>
            {
                await ClassificationsClient.CreateAsync(classifiy.Adapt<CreateClassificationRequest>());
            },
            updateFunc: async (id, classifiy) =>
            {
                await ClassificationsClient.UpdateAsync(id, classifiy.Adapt<UpdateClassificationRequest>());
            },
            deleteFunc: async id => await ClassificationsClient.DeleteAsync(id));

    // Advanced Search

}

public class ClassificationViewModel : UpdateClassificationRequest
{
 
}