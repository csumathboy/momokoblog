using csumathboy.Client.Infrastructure.ApiClient;
using csumathboy.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace csumathboy.Client.Pages.Posts;
public class TagAutoComplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<TagAutoComplete> L { get; set; } = default!;
    [Inject]
    private ITagClient TagClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<TagDto> _tag = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["Tag"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchTag;
        ToStringFunc = GetTagName;
        Clearable = true;
        return base.SetParametersAsync(parameters);
    }

    // when the value parameter is set, we have to load that one brand to be able to show the name
    // we can't do that in OnInitialized because of a strange bug (https://github.com/MudBlazor/MudBlazor/issues/3818)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender &&
            _value != default &&
            await ApiHelper.ExecuteCallGuardedAsync(
                () => TagClient.GetAsync(_value), Snackbar) is { } tag)
        {
            _tag.Add(tag.Adapt<TagDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchTag(string value)
    {
        var filter = new SearchTagRequest
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => TagClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfTagDto response)
        {
            _tag = response.Data.ToList();
        }

        return _tag.Select(x => x.Id);
    }

    private string GetTagName(Guid id) =>
        _tag.Find(b => b.Id == id)?.Name ?? string.Empty;
}