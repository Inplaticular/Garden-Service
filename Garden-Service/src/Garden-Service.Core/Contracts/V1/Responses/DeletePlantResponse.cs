namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class DeletePlantResponse : BaseResponse {
	public static class Message {
		public static readonly ValueObjects.Message
			PlantDeletionSuccessfully = new() {
				Code = nameof(PlantDeletionSuccessfully),
				Description = "The plant was deleted successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			PlantDeletionError = new() {
				Code = nameof(PlantDeletionError),
				Description = "The plant wasn't deleted successfully."
			};
		
		public static readonly ValueObjects.Message
			PlantDeletionErrorIdNotFound = new() {
				Code = nameof(PlantDeletionErrorIdNotFound),
				Description =
					"The passed plant-id didn't retrieve any entry in the database. Please consider checking the id!"
			};
	}
}