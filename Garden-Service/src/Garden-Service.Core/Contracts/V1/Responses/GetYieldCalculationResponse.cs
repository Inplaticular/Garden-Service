namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class GetYieldCalculationResponse : BaseResponse {

	public double Yield { get; set; }
	
	public static class Message {
		public static readonly ValueObjects.Message
			GetYieldCalculationSuccessfully = new() {
				Code = nameof(GetYieldCalculationSuccessfully),
				Description = "The assigned permissions were returned successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			GetYieldCalculationError = new() {
				Code = nameof(GetYieldCalculationError),
				Description = "The return of the assigned permissions wasn't successfully."
			};
	}
}