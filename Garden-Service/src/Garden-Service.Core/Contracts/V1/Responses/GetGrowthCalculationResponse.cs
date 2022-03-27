using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class GetGrowthCalculationResponse : BaseResponse {
	[Range(0, double.MaxValue)] public double GrowthPercentage { get; set; }
	[Range(0, int.MaxValue)] public int RipeTime { get; set; }

	public static class Message {
		public static readonly ValueObjects.Message
			GetGrowthCalculationSuccessfully = new() {
				Code = nameof(GetGrowthCalculationSuccessfully),
				Description = "The assigned permissions were returned successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			GetGrowthCalculationError = new() {
				Code = nameof(GetGrowthCalculationError),
				Description = "The return of the assigned permissions wasn't successfully."
			};
	}
}