using Inplanticular.Garden_Service.Core.Models.External;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Information;

public class
	GetUserClaimsForOrganizationalUnitResponse : BaseResponse<GetUserClaimsForOrganizationalUnitResponse.Body> {
	public class Body {
		public IEnumerable<OrganizationalUnitUserClaim> UserClaims { get; set; }
	}

	public static class Message {
		public static ValueObjects.Message UserClaimsReturnedSuccessfully = new() {
			Code = nameof(UserClaimsReturnedSuccessfully),
			Description = "All registered user claims for the specified organizational unit were returned successfully"
		};
	}

	public static class Error {
		public static ValueObjects.Message OrganizationalUnitDoesNotExist = new() {
			Code = nameof(OrganizationalUnitDoesNotExist),
			Description = "The specified organizational unit does not exist"
		};
	}
}