using System.IdentityModel.Tokens.Jwt;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Groups;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Authorization.Groups;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Authorization.Units;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Information;
using Inplanticular.Garden_Service.Core.Enums;
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
		_gatewayOptions = gatewayOptions.Value;
		_identityServiceOptions = identityServiceOptions.Value;
	}

	public async Task<OrganizationalGroup?> CreateOrganizationalGroupAsync(AddOrganizationalGroupRequest request) {
		using var httpClient = new HttpClient();
		var response = await httpClient.SendGetAsync<GetOrganizationalGroupByNameResponse>(_gatewayOptions.Routes
			.InformationAuthorizationGroup + "?GroupName=" + request.Name);

		if (response is null)
			return null;
		if (response.Content!.Group is not null)
			return response.Content.Group;
		var addResponse = await httpClient.SendPostAsync<AddOrganizationalGroupRequest, AddOrganizationalGroupResponse>(
			_gatewayOptions.Routes.AuthorizationGroup,
			request
		);

		if (addResponse is not null && addResponse.Succeeded)
			return addResponse.Content!.Group;

		return null;
	}

	public async Task<OrganizationalUnit?> CreateOrganizationalUnitAsync(AddOrganizationalUnitRequest request) {
		using var httpClient = new HttpClient();
		var addResponse = await httpClient.SendPostAsync<AddOrganizationalUnitRequest, AddOrganizationalUnitResponse>(
			_gatewayOptions.Routes.AuthorizationUnit,
			request
		);

		if (addResponse is not null && addResponse.Succeeded)
			return addResponse.Content!.Unit;

		return null;
	}

	public async Task DeleteOrganizationalUnitAsync(RemoveOrganizationalUnitRequest request) {
		using var httpClient = new HttpClient();
		var deleteResponse =
			await httpClient.SendDeleteAsync<RemoveOrganizationalUnitRequest, RemoveOrganizationalUnitResponse>(
				_gatewayOptions.Routes.AuthorizationUnit,
				request
			);
	}

	public bool CheckUserHasId(string token, string userId) {
		var uid = GetUserIdFromToken(token);
		return uid is not null && uid == userId;
	}

	public async Task<bool> CheckUserHasAnyRole(string token, string unitId,
		IEnumerable<GardenRoles> gardenRoles) {
		var userId = GetUserIdFromToken(token);
		if (userId is null) return false;
		using var httpClient = new HttpClient();
		var response = await httpClient.SendGetAsync<GetUserClaimsForOrganizationalUnitResponse>(
			string.Format("{0}?UnitId={1}", _gatewayOptions.Routes.InformationAuthorizationUserClaims, unitId));
		if (response is null || !response.Succeeded)
			return false;

		return response.Content!.UserClaims.Any(claim =>
			claim.UserId.Equals(userId) && claim.Type.Equals(UserClaimTypes.Role.ToString()) &&
			gardenRoles.Select(r => r.ToString()).ToList().Contains(claim.Value));
	}

	public async Task<bool> CheckPermissionBelongsToUnit(string permissionId, string unitId) {
		using var httpClient = new HttpClient();
		var response = await httpClient.SendGetAsync<GetUserClaimsForOrganizationalUnitResponse>(
			string.Format("{0}?UnitId={1}", _gatewayOptions.Routes.InformationAuthorizationUserClaims, unitId));
		if (response is null || !response.Succeeded)
			return false;

		return response.Content!.UserClaims.Any(claim => claim.Id.Equals(permissionId));
	}

	private string? GetUserIdFromToken(string token) {
		var securityToken = new JwtSecurityTokenHandler().ReadToken(token);
		if (securityToken is not JwtSecurityToken jwtSecurityToken) return null;
		return jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type.Equals(JwtRegisteredClaimNames.Sub))?.Value;
	}
}