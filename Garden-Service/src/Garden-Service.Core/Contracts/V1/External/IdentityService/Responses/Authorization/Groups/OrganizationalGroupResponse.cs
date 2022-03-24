using Inplanticular.Garden_Service.Core.Models.External;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses.Authorization.Groups;

public class OrganizationalGroupResponse<TBody> : BaseResponse<TBody>
	where TBody : OrganizationalGroupResponse<TBody>.Body {
	public class Body {
		public OrganizationalGroup? Group { get; set; }
	}
}