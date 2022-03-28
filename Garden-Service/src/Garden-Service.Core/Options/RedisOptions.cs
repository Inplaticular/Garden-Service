namespace Inplanticular.Garden_Service.Core.Options; 

public class RedisOptions {
	public const string AppSettingsKey = nameof(RedisOptions);

	public string ProviderName { get; set; }
	public string Host { get; set; }
	public int Port { get; set; }

	public bool AllowAdmin { get; set; }
	public int SyncTimeout { get; set; }
	public int AsyncTimeout { get; set; }
	public bool IsHybridCache { get; set; }
	public bool DisableLogging { get; set; }
} 