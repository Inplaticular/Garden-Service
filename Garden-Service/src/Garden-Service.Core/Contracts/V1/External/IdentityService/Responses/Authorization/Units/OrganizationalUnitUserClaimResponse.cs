using Inplanticular.Garden_Service.Core.Models.External;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Authorization.Units;

public class OrganizationalUnitUserClaimResponse<TBody> : BaseResponse<TBody>
	where TBody : OrganizationalUnitUserClaimResponse<TBody>.Body {
	public class Body {
		public OrganizationalUnitUserClaim UserClaim { get; set; }
	}
}