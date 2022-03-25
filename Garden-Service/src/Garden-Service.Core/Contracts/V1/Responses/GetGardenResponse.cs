using Inplanticular.Garden_Service.Core.Models;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class GetGardenResponse : BaseResponse {
	public IEnumerable<Garden> Gardens { get; set; }

	public static class Message {
		public static readonly ValueObjects.Message
			GardenReturnSuccessfully = new() {
				Code = nameof(GardenReturnSuccessfully),
				Description = "The gardens were returned successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			GardenReturnError = new() {
				Code = nameof(GardenReturnError),
				Description = "The return of the gardens wasn't successfully."
			};
	}
}