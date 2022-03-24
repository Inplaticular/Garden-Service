using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Requests.Authorization;

public class AuthorizeRequest {
	[Required] public string Token { get; set; }
}