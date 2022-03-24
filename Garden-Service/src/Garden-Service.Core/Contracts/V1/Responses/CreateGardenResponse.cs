namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class CreateGardenResponse : BaseResponse {
	public string GardenId { get; set; }

	public static class Message {
		public static readonly ValueObjects.Message
			GardenCreationSuccessfully = new() {
				Code = nameof(GardenCreationSuccessfully),
				Description = "The garden was created successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			GardenCreationError = new() {
				Code = nameof(GardenCreationError),
				Description = "The creation of the garden wasn't successfully."
			},
			MissingOrganizationalGroup = new() {
				Code = nameof(MissingOrganizationalGroup),
				Description = "There is no organizational group and the creation of the group failed"
			},
			OrganizationalUnitCreationFailed = new() {
				Code = nameof(OrganizationalUnitCreationFailed),
				Description = "The creation of the organizational unit failed"
			},
			PermissionOfGardenCreationFailed = new() {
				Code = nameof(OrganizationalUnitCreationFailed),
				Description = "The creation of the organizational unit failed"
			};
	}
}