namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class DeletePermissionForGardenResponse : BaseResponse {
	public static class Message {
		public static readonly ValueObjects.Message
			DeletePermissionForGardenSuccessfully = new() {
				Code = nameof(DeletePermissionForGardenSuccessfully),
				Description = "The permission for the garden was deleted successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			DeletePermissionForGardenError = new() {
				Code = nameof(DeletePermissionForGardenError),
				Description = "The deletion of the permission of the garden wasn't successfully."
			},
			DeletePermissionForGardenErrorBadRequest = new() {
				Code = nameof(DeletePermissionForGardenErrorBadRequest),
				Description =
					"The deletion of the permission for the garden wasn't successfully. Your request did not contain the right type or value attribute. Consider checking both!"
			},
			DeletePermissionForGardenErrorIdMismatch = new() {
				Code = nameof(DeletePermissionForGardenErrorBadRequest),
				Description =
					"The deletion of the permission for the garden wasn't successfully. Your permissionId did not belong to the passed unitId. Consider checking both!"
			},
			DeletionNotAllowed = new() {
				Code = nameof(DeletionNotAllowed),
				Description = "It is not allowed to delete the Owner permission of the garden"
			};
	}
}