using csumathboy.Client.Infrastructure.ApiClient;
using csumathboy.Client.Shared;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace csumathboy.Client.Pages.Posts;
public class ClassAutoComplete : MudAutocomplete<Guid>
{
    [Inject]
    private IStringLocalizer<ClassAutoComplete> L { get; set; } = default!;
    [Inject]
    private IClassificationClient ClassificationClient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    private List<ClassificationDto> _classification = new();

    // supply default parameters, but leave the possibility to override them
    public override Task SetParametersAsync(ParameterView parameters)
    {
        Label = L["Classification"];
        Variant = Variant.Filled;
        Dense = true;
        Margin = Margin.Dense;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchClassification;
        ToStringFunc = GetClassificationName;
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
                () => ClassificationClient.GetAsync(_value), Snackbar) is { } classification)
        {
            _classification.Add(classification.Adapt<ClassificationDto>());
            ForceRender(true);
        }
    }

    private async Task<IEnumerable<Guid>> SearchClassification(string value)
    {
        var filter = new SearchClassificationRequest
        {
            PageSize = 10,
            AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => ClassificationClient.SearchAsync(filter), Snackbar)
            is PaginationResponseOfClassificationDto response)
        {
            _classification = response.Data.ToList();
        }

        return _classification.Select(x => x.Id);
    }

    private string GetClassificationName(Guid id) =>
        _classification.Find(b => b.Id == id)?.Name ?? string.Empty;
}