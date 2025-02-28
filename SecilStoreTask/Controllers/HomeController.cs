using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecilStoreTask.Models;
using System.Linq;
using System.Threading.Tasks;

public class ConfigurationController : Controller
{
	private readonly ConfigurationDbContext _dbContext;

	public ConfigurationController(ConfigurationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	// ?? 1. Konfigürasyonlarý Listeleme
	public async Task<IActionResult> Index()
	{
		var configurations = await _dbContext.ConfigurationRecords.ToListAsync();
		return View(configurations);
	}

	// ?? 2. Yeni Konfigürasyon Ekleme Formu
	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Create(ConfigurationRecords record)
	{
		if (ModelState.IsValid)
		{
			_dbContext.ConfigurationRecords.Add(record);
			await _dbContext.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		return View(record);
	}

	// ?? 3. Konfigürasyon Düzenleme Formu
	public async Task<IActionResult> Edit(int id)
	{
		var record = await _dbContext.ConfigurationRecords.FindAsync(id);
		if (record == null)
		{
			return NotFound();
		}
		return View(record);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(int id, ConfigurationRecords record)
	{
		if (id != record.Id) return BadRequest();

		if (ModelState.IsValid)
		{
			_dbContext.Update(record);
			await _dbContext.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		return View(record);
	}

	// ?? 4. Konfigürasyon Silme
	public async Task<IActionResult> Delete(int id)
	{
		var record = await _dbContext.ConfigurationRecords.FindAsync(id);
		if (record == null) return NotFound();

		_dbContext.ConfigurationRecords.Remove(record);
		await _dbContext.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
}
