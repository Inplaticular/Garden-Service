namespace Inplanticular.Garden_Service.Core.Options;

public class GatewayOptions {
	public const string AppSettingsKey = nameof(GatewayOptions);
	public Routes Routes { get; set; }
}

public class Routes {
	public string AuthorizeUser { get; set; }
	public string AuthorizeUserClaim { get; set; }
	public string InformationAuthorizationGroup { get; set; }
	public string AuthorizeGroup { get; set; }
	public string AuthorizeUnit { get; set; }
	public string AuthorizeUnitUserClaim { get; set; }
}