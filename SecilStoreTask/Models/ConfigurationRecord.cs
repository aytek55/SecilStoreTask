namespace SecilStoreTask.Models
{
	public class ConfigurationRecords
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }  // "string", "int", "bool", "double" gibi değerleri saklayacak
		public string Value { get; set; } // Değerler string olarak saklanacak, dönüşüm yapılacak
		public bool IsActive { get; set; }
		public string ApplicationName { get; set; }
	}

}
