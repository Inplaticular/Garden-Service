namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Authorization.Units;

public class
	ValidateOrganizationalUnitUserClaimResponse : BaseResponse<ValidateOrganizationalUnitUserClaimResponse.Body> {
	public class Body {
		public bool Valid { get; set; }
	}
}