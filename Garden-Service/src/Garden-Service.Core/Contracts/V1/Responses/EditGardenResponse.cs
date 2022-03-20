namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class EditGardenResponse : BaseResponse {
	public static class Message {
		public static readonly ValueObjects.Message
			GardenAlterationSuccessfully = new() {
				Code = nameof(GardenAlterationSuccessfully),
				Description = "The garden was altered successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			GardenAlterationErrorGeneral = new() {
				Code = nameof(GardenAlterationErrorGeneral),
				Description = "The alteration of the garden wasn't successfully."
			};

		public static readonly ValueObjects.Message GardenAlterationErrorIdNotFound = new() {
			Code = nameof(GardenAlterationErrorIdNotFound),
			Description =
				"The passed garden-id didn't retrieve any entry in the database. Please consider checking the id!"
		};
	}
}