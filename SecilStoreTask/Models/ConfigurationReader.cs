using Microsoft.EntityFrameworkCore;

namespace SecilStoreTask.Models
{
	public class ConfigurationReader : IDisposable
	{
		private readonly ConfigurationDbContext _dbContext;
		private readonly string _applicationName;
		private readonly int _refreshInterval;
		private readonly Timer _refreshTimer;
		private Dictionary<string, string> _configCache = new();
		private readonly object _lock = new();

		public ConfigurationReader(string applicationName, string connectionString, int refreshInterval)
		{
			_applicationName = applicationName;
			_refreshInterval = refreshInterval;

			var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
			optionsBuilder.UseSqlServer(connectionString);
			_dbContext = new ConfigurationDbContext(optionsBuilder.Options);

			LoadConfiguration(); // İlk açılışta verileri yükle
			_refreshTimer = new Timer(RefreshConfiguration, null, refreshInterval, refreshInterval);
		}

		private void LoadConfiguration()
		{
			lock (_lock)
			{
				_configCache = _dbContext.ConfigurationRecords
					.Where(c => c.ApplicationName == "SERVICE-A" && c.IsActive)
					.ToDictionary(c => c.Name, c => c.Value);
			}
		}

		private void RefreshConfiguration(object? state)
		{
			try
			{
				LoadConfiguration();
			}
			catch (Exception ex)
			{
				// Hata durumunda loglama eklenebilir
			}
		}

		public T GetValue<T>(string key)
		{
			if (_configCache.TryGetValue(key, out var value))
			{
				return (T)Convert.ChangeType(value, typeof(T));
			}
			//else if (!_configCache.TryGetValue(key, out var value))
			//{
			//	// Anahtar bulunamadığında bir log yazabilirsiniz
			//	Console.WriteLine($"Configuration key '{key}' not found.");
			//	return default!;  // Hata fırlatmak yerine varsayılan değeri döndür
			//}
			return default;
		}

		public void Dispose()
		{
			_refreshTimer?.Dispose();
			_dbContext?.Dispose();
		}
	}

}
