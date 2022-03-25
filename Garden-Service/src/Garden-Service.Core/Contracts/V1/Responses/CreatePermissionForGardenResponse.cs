using Inplanticular.Garden_Service.Core.Models.External;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class CreatePermissionForGardenResponse : BaseResponse {
	public OrganizationalUnitUserClaim PermissionForGarden { get; set; }

	public static class Message {
		public static readonly ValueObjects.Message
			CreatePermissionForGardenSuccessfully = new() {
				Code = nameof(CreatePermissionForGardenSuccessfully),
				Description = "The permission for the garden was created successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			CreatePermissionForGardenError = new() {
				Code = nameof(CreatePermissionForGardenError),
				Description = "The creation of the permission of the garden wasn't successfully."
			},
			CreatePermissionForGardenErrorBadRequest = new() {
				Code = nameof(CreatePermissionForGardenErrorBadRequest),
				Description =
					"The creation of the permission for the garden wasn't successfully. Your request did not contain the right type or value attribute. Consider checking both!"
			};
	}
}