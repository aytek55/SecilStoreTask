using Microsoft.EntityFrameworkCore;
using SecilStoreTask.Models;

namespace SecilStoreTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var builder = WebApplication.CreateBuilder(args);

			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

			var applicationName = builder.Configuration["ApplicationName"] ?? "DefaultApp";
			var refreshInterval = int.Parse(builder.Configuration["RefreshInterval"] ?? "60000");

			builder.Services.AddSingleton(sp => new ConfigurationReader(applicationName, connectionString, refreshInterval));
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<ConfigurationDbContext>(options => options.UseSqlServer(connectionString));

			var app = builder.Build();

			app.UseRouting();
			app.UseAuthorization();

			// Burayý ekleyin!
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

			app.Run();

		}
	}
}
