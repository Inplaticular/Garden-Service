namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class CreatePlantResponse : BaseResponse {
	public int PlantId { get; set; }

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
			};
	}
}