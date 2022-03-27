using Inplanticular.Garden_Service.Core.Contracts.V1.ValueObjects;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.CalculationService.Responses;

public class BaseResponse {
	public bool Succeeded { get; set; } = false;
	public IEnumerable<Message> Messages { get; set; } = Enumerable.Empty<Message>();
	public IEnumerable<Message> Errors { get; set; } = Enumerable.Empty<Message>();
}