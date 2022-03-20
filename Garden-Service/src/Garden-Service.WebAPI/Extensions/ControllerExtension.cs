using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;
using Inplanticular.Garden_Service.Core.Contracts.V1.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Inplanticular.Garden_Service.WebAPI.Extensions;

public class ControllerExtension {
	public static IActionResult ErrorResponse<TResponse>(ControllerBase controller, Exception exception)
		where TResponse : BaseResponse {
		var response = Activator.CreateInstance<TResponse>();
		response.Errors = new[] {
			new Message {
				Code = exception.GetType().Name,
				Description = exception.Message
			}
		};
		return controller.StatusCode(500, response);
	}
}