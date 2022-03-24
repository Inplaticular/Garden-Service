using Inplanticular.Garden_Service.Core.Models.External;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Authorization.Units;

public class OrganizationalUnitResponse<TBody> : BaseResponse<TBody>
	where TBody : OrganizationalUnitResponse<TBody>.Body {
	public class Body {
		public OrganizationalUnit Unit { get; set; }
	}
}