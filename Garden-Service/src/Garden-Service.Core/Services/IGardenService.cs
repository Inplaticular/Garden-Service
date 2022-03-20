using Inplanticular.Garden_Service.Core.Contracts.V1.Requests;
using Inplanticular.Garden_Service.Core.Contracts.V1.Responses;

namespace Inplanticular.Garden_Service.Core.Services;

public interface IGardenService {
	Task<CreateGardenResponse> CreateGardenAsync(CreateGardenRequest request);

	Task<DeleteGardenResponse> DeleteGardenAsync(DeleteGardenRequest request);

	Task<EditGardenResponse> EditGardenAsync(EditGardenRequest request);
}