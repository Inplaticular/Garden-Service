namespace Inplanticular.Garden_Service.Core.Options;

public class IdentityServiceOptions {
	public const string AppSettingsKey = nameof(IdentityServiceOptions);
	public string OrganizationalGroupName { get; set; }
}