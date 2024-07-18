using csumathboy.Client.Components.EntityTable;
using csumathboy.Client.Infrastructure.ApiClient;
using csumathboy.Client.Infrastructure.Common;
using csumathboy.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace csumathboy.Client.Pages.Posts;
public partial class Comments
{
    [Inject]
    protected ICommentClient CommentsClient { get; set; } = default!;

    protected EntityServerTableContext<CommentDto, Guid, CommentViewModel> Context { get; set; } = default!;

    private EntityTable<CommentDto, Guid, CommentViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new(
            entityName: L["Comment"],
            entityNamePlural: L["Comments"],
            entityResource: FSHResource.Comment,
            fields: new()
            {
                new(commt => commt.Id, L["Id"], "Id"),
                new(commt => commt.Title, L["Title"], "Title"),
                new(commt => commt.Email, L["Email"], "Email"),
                new(commt => commt.RealName, L["RealName"], "RealName"),
                new(commt => commt.PostsId, L["PostsId"], "PostsId"),
                new(commt => commt.PhoneNumber, L["PhoneNumber"], "PhoneNumber")
            },
            enableAdvancedSearch: false,
            idFunc: commt => commt.Id,
            searchFunc: async filter =>
            {
                var commentFilter = filter.Adapt<SearchCommentRequest>();
 
                var result = await CommentsClient.SearchAsync(commentFilter);
                return result.Adapt<PaginationResponse<CommentDto>>();
            },
            getDetailsFunc: async (id) =>
             {
                 var commentDetail = await CommentsClient.GetAsync(id);
                 return new CommentViewModel()
                 {
                     Id = id,
                     RealName = commentDetail.RealName,
                     PostsId = commentDetail.PostsId,
                     Email = commentDetail.Email,
                     PhoneNumber = commentDetail.PhoneNumber,
                     Title = commentDetail.Title,
                     Description = commentDetail.Description
                 };
             },
            createFunc: async commt =>
            {
                await CommentsClient.CreateAsync(commt.Adapt<CreateCommentRequest>());
            },
            updateFunc: async (id, commt) =>
            {
                await CommentsClient.UpdateAsync(id, commt.Adapt<UpdateCommentRequest>());
            },
            deleteFunc: async id => await CommentsClient.DeleteAsync(id));

    // Advanced Search

}

public class CommentViewModel : UpdateCommentRequest
{
 
}