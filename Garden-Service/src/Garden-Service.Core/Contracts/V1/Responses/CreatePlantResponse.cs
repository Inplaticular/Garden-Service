namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class CreatePlantResponse : BaseResponse {
	public string PlantId { get; set; }

	public static class Message {
		public static readonly ValueObjects.Message
			PlantCreationSuccessfully = new() {
				Code = nameof(PlantCreationSuccessfully),
				Description = "The plant was created successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			PlantCreationError = new() {
				Code = nameof(PlantCreationError),
				Description = "The creation of the plant wasn't successfully."
			},
			MissingOrganizationalGroup = new() {
				Code = nameof(MissingOrganizationalGroup),
				Description = "There is no organizational group and the creation of the group failed"
			},
			OrganizationalUnitCreationFailed = new() {
				Code = nameof(OrganizationalUnitCreationFailed),
				Description = "The creation of the organizational unit failed"
			};
	}
}