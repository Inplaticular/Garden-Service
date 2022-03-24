using Inplanticular.Garden_Service.Core.Contracts.V1.ValueObjects;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses;

public class BaseResponse<TBody> : BaseResponse {
	public new TBody? Content { get; set; }
}

public class BaseResponse {
	public bool Succeeded { get; set; } = false;
	public IEnumerable<Message> Messages { get; set; } = Enumerable.Empty<Message>();
	public IEnumerable<Message> Errors { get; set; } = Enumerable.Empty<Message>();
	public object? Content { get; set; } = null;
}