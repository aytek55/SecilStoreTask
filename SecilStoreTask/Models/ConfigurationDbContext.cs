using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SecilStoreTask.Models
{
	public class ConfigurationDbContext : DbContext
	{
		public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options)
		: base(options) { }

		public DbSet<ConfigurationRecords> ConfigurationRecords { get; set; }
	}

}
