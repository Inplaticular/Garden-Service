namespace Inplanticular.Garden_Service.Core.Contracts.V1.Requests;

public record CreatePlantRequest {
	public string BotanicalName { get; set; }

	public string GardenId { get; set; }
}