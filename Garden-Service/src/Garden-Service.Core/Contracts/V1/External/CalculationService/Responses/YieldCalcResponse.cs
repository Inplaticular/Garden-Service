using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.CalculationService.Responses;

public class YieldCalcResponse : BaseResponse {
	[Required] public double Yield { get; set; }


	public static class Message {
		public static readonly ValueObjects.Message
			YieldCalculationSuccessful = new() {
				Code = nameof(YieldCalculationSuccessful),
				Description = "The yield was estimated successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			PlantDiedError = new() {
				Code = nameof(PlantDiedError),
				Description = "The yield wasnt estimated successfully."
			};
	}
}