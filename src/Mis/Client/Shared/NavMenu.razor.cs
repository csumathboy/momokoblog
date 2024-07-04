using csumathboy.Client.Infrastructure.Auth;
using csumathboy.Client.Infrastructure.Common;
using csumathboy.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace csumathboy.Client.Shared;
public partial class NavMenu
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = default!;

    private string? _hangfireUrl;
    private bool _canViewHangfire;
    private bool _canViewDashboard;
    private bool _canViewRoles;
    private bool _canViewUsers;
    private bool _canViewProducts;
    private bool _canViewBrands;
    private bool _canViewPosts;
    private bool _canViewClassifications;
    private bool _canViewTags;
    private bool _canViewComments;
    private bool _canViewTenants;
    private bool CanViewAdministrationGroup => _canViewUsers || _canViewRoles || _canViewTenants;

    protected override async Task OnParametersSetAsync()
    {
        _hangfireUrl = Config[ConfigNames.ApiBaseUrl] + "jobs";
        var user = (await AuthState).User;
        _canViewHangfire = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Hangfire);
        _canViewDashboard = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Dashboard);
        _canViewRoles = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Roles);
        _canViewUsers = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Users);
        _canViewProducts = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Products);
        _canViewBrands = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Brands);
        _canViewTenants = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Tenants);
        _canViewPosts = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Post);
        _canViewClassifications = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Classification);
        _canViewTags = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Tag);
        _canViewComments = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Comment);
    }
}