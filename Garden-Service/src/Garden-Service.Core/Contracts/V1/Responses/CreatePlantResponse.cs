namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class CreatePlantResponse : BaseResponse {
	public int PlantId { get; set; }

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
			};
	}
}