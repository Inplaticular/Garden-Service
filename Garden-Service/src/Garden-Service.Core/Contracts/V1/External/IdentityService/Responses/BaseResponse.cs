using Inplanticular.Garden_Service.Core.Contracts.V1.ValueObjects;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.IdentityService.Responses;

public class BaseResponse<TBody> : BaseResponse {
	public new TBody? Content { get; set; }
}

public class BaseResponse {
	public bool Succeeded { get; set; } = false;
	public ICollection<Message> Messages { get; set; } = Array.Empty<Message>();
	public ICollection<Message> Errors { get; set; } = Array.Empty<Message>();
	public object? Content { get; set; } = null;
}