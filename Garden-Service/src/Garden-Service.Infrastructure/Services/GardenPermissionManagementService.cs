using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization.Units;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Authorization.Units;
using Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Information;
using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Enums;
using Inplanticular.Garden_Service.Core.Options;
using Inplanticular.Garden_Service.Core.Services;
using Inplanticular.Garden_Service.Infrastructure.Extensions;
using Microsoft.Extensions.Options;

namespace Inplanticular.Garden_Service.Infrastructure.Services;

public class GardenPermissionManagementService : IGardenPermissionManagementService {
	private readonly GatewayOptions _gatewayOptions;

	public GardenPermissionManagementService(IOptions<GatewayOptions> gatewayOptions) {
		_gatewayOptions = gatewayOptions.Value;
	}

	public GetAssignableRolesResponse GetAssignableRoles() {
		return new GetAssignableRolesResponse {
			Succeeded = true,
			GardenRoles = new List<string> {GardenRoles.Collaborator.ToString(), GardenRoles.Visitor.ToString()},
			Messages = new[] {GetAssignableRolesResponse.Message.GardenRolesReturnSuccessfully}
		};
	}

	public async Task<GetAssignedPermissionsForGardenResponse> GetPermissionsForGardenAsync(
		GetAssignedPermissionsForGardenRequest request) {
		using var httpClient = new HttpClient();
		var response = await httpClient.SendGetAsync<GetUserClaimsForOrganizationalUnitResponse>(
			string.Format("{0}?UnitId={1}", _gatewayOptions.Routes.InformationAuthorizationUserClaims, request.UnitId));

		if (response is null || response.Succeeded is false) {
			if (response != null)
				return
					new GetAssignedPermissionsForGardenResponse {
						Succeeded = false,
						Errors = new[] {GetAssignedPermissionsForGardenResponse.Error.AssignedPermissionsReturnError}
							.Concat(response.Errors)
					};
			return
				new GetAssignedPermissionsForGardenResponse {
					Succeeded = false,
					Errors = new[] {GetAssignedPermissionsForGardenResponse.Error.AssignedPermissionsReturnError}
				};
		}

		return new GetAssignedPermissionsForGardenResponse {
			Succeeded = true,
			AssignedPermissionsForGarden = response.Content!.UserClaims,
			Messages = new[] {
				GetAssignedPermissionsForGardenResponse.Message.AssignedPermissionsReturnSuccessfully
			}
		};
	}

	public async Task<CreatePermissionForGardenResponse> CreatePermissionForGardenAsync(
		CreatePermissionForGardenRequest request) {
		using var httpClient = new HttpClient();
		if (request.Type != "Role" || !GetAssignableRoles().GardenRoles.Contains(request.Value))
			return
				new CreatePermissionForGardenResponse {
					Succeeded = false,
					Errors =
						new[] {CreatePermissionForGardenResponse.Error.CreatePermissionForGardenErrorBadRequest}
				};

		var response =
			await httpClient
				.SendPostAsync<CreatePermissionForGardenRequest, AddOrganizationalUnitUserClaimResponse>(
					_gatewayOptions.Routes.AuthorizationUnitUserClaim, request);

		if (response is null || response.Succeeded is false) {
			if (response != null)
				return
					new CreatePermissionForGardenResponse {
						Succeeded = false,
						Errors =
							new[] {CreatePermissionForGardenResponse.Error.CreatePermissionForGardenError}.Concat(
								response.Errors)
					};
			return
				new CreatePermissionForGardenResponse {
					Succeeded = false,
					Errors =
						new[] {CreatePermissionForGardenResponse.Error.CreatePermissionForGardenError}
				};
		}

		return new CreatePermissionForGardenResponse {
			Succeeded = true,
			PermissionForGarden = response.Content.UserClaim,
			Messages = new[] {
				CreatePermissionForGardenResponse.Message.CreatePermissionForGardenSuccessfully
			}
		};
	}

	public async Task<DeletePermissionForGardenResponse> DeletePermissionForGardenAsync(DeletePermissionForGardenRequest request) {
		using var httpClient = new HttpClient();
		Console.WriteLine(request.Id);
		var removeOrganizationalUnitUserClaimRequest = new RemoveOrganizationalUnitUserClaimRequest {
			Id = request.Id
		};
		Console.WriteLine(removeOrganizationalUnitUserClaimRequest.Id);
		var response =
			await httpClient
				.SendDeleteAsync<RemoveOrganizationalUnitUserClaimRequest, RemoveOrganizationalUnitUserClaimResponse>(
					_gatewayOptions.Routes.AuthorizationUnitUserClaim, removeOrganizationalUnitUserClaimRequest);

		if (response is null || response.Succeeded is false) {
			if (response != null)
				return
					new DeletePermissionForGardenResponse {
						Succeeded = false,
						Errors =
							new[] {DeletePermissionForGardenResponse.Error.DeletePermissionForGardenError}.Concat(
								response.Errors)
					};
			return
				new DeletePermissionForGardenResponse {
					Succeeded = false,
					Errors =
						new[] {DeletePermissionForGardenResponse.Error.DeletePermissionForGardenError}
				};
		}

		return new DeletePermissionForGardenResponse {
			Succeeded = true,
			Messages = new[] {
				DeletePermissionForGardenResponse.Message.DeletePermissionForGardenSuccessfully
			}
		};
	}
}