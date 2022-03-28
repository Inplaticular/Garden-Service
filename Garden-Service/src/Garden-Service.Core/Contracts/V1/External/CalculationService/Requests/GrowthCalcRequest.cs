using System.ComponentModel.DataAnnotations;

namespace Inplanticular.Garden_Service.Core.Contracts.V1.External.CalculationService.Requests;

/// <summary>
///     Record, that contains information of the plant, of which the growth is going to be calculated.
/// </summary>
public record GrowthCalcRequest {
	public double PlantCoordinateLatitude { get; set; }
	public double PlantCoordinateLongitude { get; set; }

	/// <summary>
	///     Defines how many days are passed, since the plant was planted.
	/// </summary>
	[Range(1, int.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
	public int TimeFromPlanting { get; set; }

	/// <summary>
	///     Defines how much the plant was grown yesterday in percent.
	/// </summary>
	[Range(1, int.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
	public double RipePercentageYesterday { get; set; }

	/// <summary>
	///     Defines how much the plant grows each day in percent.
	/// </summary>
	[Range(0, double.MaxValue, ErrorMessage = "Value should be greater than or equal to 0")]
	public double GrowthPerDay { get; set; }

	/// <summary>
	///     Defines how much how fertilizer enhances the growth of the plant.
	/// </summary>
	[Required]
	public double FertilizerPercentageToday { get; set; }

	/// <summary>
	///     Defines how many days a plant got no water in row.
	/// </summary>
	[Required]
	[Range(0, int.MaxValue)]
	public int DaysWithoutWater { get; set; }
}