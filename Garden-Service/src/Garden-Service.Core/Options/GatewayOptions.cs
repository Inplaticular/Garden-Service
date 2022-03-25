namespace Inplanticular.Garden_Service.Core.Options;

public class GatewayOptions {
	public const string AppSettingsKey = nameof(GatewayOptions);
	public Routes Routes { get; set; }
}

public class Routes {
	public string AuthorizeUser { get; set; }
	public string AuthorizeUserClaim { get; set; }
	public string InformationAuthorizationGroup { get; set; }
	
	public string InformationAuthorizationUserClaims { get; set; }
	public string AuthorizationGroup { get; set; }
	public string AuthorizationUnit { get; set; }
	public string AuthorizationUnitUserClaim { get; set; }
}