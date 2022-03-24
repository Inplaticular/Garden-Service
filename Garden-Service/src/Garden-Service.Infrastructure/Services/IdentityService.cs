using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Groups;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Authorization.Groups;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Authorization.Units;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Information;
using Inplanticular.Garden_Service.Core.Models.External;
using Inplanticular.Garden_Service.Core.Options;
using Inplanticular.Garden_Service.Core.Services;
using Inplanticular.Garden_Service.Infrastructure.Extensions;
using Microsoft.Extensions.Options;

namespace Inplanticular.Garden_Service.Infrastructure.Services;

public class IdentityService : IIdentityService {
	private readonly GatewayOptions _gatewayOptions;
	private readonly IdentityServiceOptions _identityServiceOptions;

	public IdentityService(IOptions<GatewayOptions> gatewayOptions,
		IOptions<IdentityServiceOptions> identityServiceOptions) {
		this._gatewayOptions = gatewayOptions.Value;
		this._identityServiceOptions = identityServiceOptions.Value;
	}

	public async Task<OrganizationalGroup?> CreateOrganizationalGroupAsync(AddOrganizationalGroupRequest request) {
		using var httpClient = new HttpClient();
		var response = await httpClient.SendGetAsync<GetOrganizationalGroupByNameResponse>(_gatewayOptions.Routes
			.InformationAuthorizationGroup + "?GroupName=" + request.Name);

		if (response is null)
			return null;
		if (response.Content!.Group is not null)
			return response.Content.Group;
		var addResponse = await httpClient.SendPutAsync<AddOrganizationalGroupRequest, AddOrganizationalGroupResponse>(
			this._gatewayOptions.Routes.AuthorizeGroup,
			request
		);

		if (addResponse is not null && addResponse.Succeeded)
			return addResponse.Content!.Group;

		return null;
	}

	public async Task<OrganizationalUnit?> CreateOrganizationalUnitAsync(AddOrganizationalUnitRequest request) {
		using var httpClient = new HttpClient();
		var addResponse = await httpClient.SendPutAsync<AddOrganizationalUnitRequest, AddOrganizationalUnitResponse>(
			this._gatewayOptions.Routes.AuthorizeUnit,
			request
		);

		if (addResponse is not null && addResponse.Succeeded)
			return addResponse.Content!.Unit;

		return null;
	}

	public async Task DeleteOrganizationalUnitAsync(RemoveOrganizationalUnitRequest request) {
		using var httpClient = new HttpClient();
		var deleteResponse = await httpClient.SendDeleteAsync<RemoveOrganizationalUnitRequest, RemoveOrganizationalUnitResponse>(
			this._gatewayOptions.Routes.AuthorizeUnit,
			request
		);
	}
}