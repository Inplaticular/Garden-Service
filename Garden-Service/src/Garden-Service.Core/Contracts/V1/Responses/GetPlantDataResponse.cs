using Inplanticular.Garden_Service.Core.Models;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class GetPlantDataResponse : BaseResponse {
	public List<PlantData> PlantDataList { get; set; }

	public static class Message {
		public static readonly ValueObjects.Message
			PlantDataReturnSuccessfully = new() {
				Code = nameof(PlantDataReturnSuccessfully),
				Description = "The plant data was returned successfully."
			};
	}

	public static class Error {
		public static readonly ValueObjects.Message
			PlantDataReturnError = new() {
				Code = nameof(PlantDataReturnError),
				Description = "The return of the plant data wasn't successfully."
			};
	}
}