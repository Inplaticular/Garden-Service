namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record GetAssignedPermissionsForGardenRequest {
	public string GardenId { get; set; }
}