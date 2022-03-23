using Inplanticular.Garden_Service.Core.Models;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class GetSingleGardenResponse : BaseResponse {
	public Garden Garden { get; set; }

	public static class Message {
		public static readonly ValueObjects.Message
			SingleGardenReturnSuccessfully = new() {
				Code = nameof(SingleGardenReturnSuccessfully),
				Description = "The garden was returned successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			SingleGardenReturnError = new() {
				Code = nameof(SingleGardenReturnError),
				Description = "The return of the garden wasn't successfully."
			},
			SingleGardenReturnErrorIdNotFound = new() {
				Code = nameof(SingleGardenReturnErrorIdNotFound),
				Description = "The return of the garden wasn't successfully."
			};
	}
}