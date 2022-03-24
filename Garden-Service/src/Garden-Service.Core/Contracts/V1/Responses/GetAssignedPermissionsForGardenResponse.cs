using Inplanticular.Garden_Service.Core.Models.External;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class GetAssignedPermissionsForGardenResponse : BaseResponse {
	public IEnumerable<OrganizationalUnitUserClaim> AssignedPermissionsForGarden { get; set; }


	public static class Message {
		public static readonly ValueObjects.Message
			AssignedPermissionsReturnSuccessfully = new() {
				Code = nameof(AssignedPermissionsReturnSuccessfully),
				Description = "The assigned permissions were returned successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			AssignedPermissionsReturnError = new() {
				Code = nameof(AssignedPermissionsReturnError),
				Description = "The return of the assigned permissions wasn't successfully."
			};
	}
}