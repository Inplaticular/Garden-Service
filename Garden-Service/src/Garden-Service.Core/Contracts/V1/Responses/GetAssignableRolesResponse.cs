namespace Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

public class GetAssignableRolesResponse : BaseResponse {
	public ICollection<string> GardenRoles { get; set; }

	public static class Message {
		public static readonly ValueObjects.Message
			GardenRolesReturnSuccessfully = new() {
				Code = nameof(GardenRolesReturnSuccessfully),
				Description = "The gardens roles were returned successfully."
			};
	}
}