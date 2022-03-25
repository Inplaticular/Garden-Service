using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

namespace Inplanticular.Garden_Service.Core.Services;

public interface IPlantService {
	Task<CreatePlantResponse> CreatePlantAsync(CreatePlantRequest request);

	Task<DeletePlantResponse> DeletePlantAsync(DeletePlantRequest request);

	Task<GetPlantDataResponse> GetPlantDataAsync();
}