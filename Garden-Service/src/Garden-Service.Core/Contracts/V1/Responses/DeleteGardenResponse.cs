namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class DeleteGardenResponse : BaseResponse {
	public static class Message {
		public static readonly ValueObjects.Message
			GardenDeletionSuccessfully = new() {
				Code = nameof(GardenDeletionSuccessfully),
				Description = "The garden was deleted successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			GardenDeletionError = new() {
				Code = nameof(GardenDeletionError),
				Description = "The garden wasn't deleted successfully."
			};

		public static readonly ValueObjects.Message
			GardenDeletionErrorIdNotFound = new() {
				Code = nameof(GardenDeletionErrorIdNotFound),
				Description =
					"The passed garden-id didn't retrieve any entry in the database. Please consider checking the id!"
			};
	}
}