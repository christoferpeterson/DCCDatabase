namespace DCCDatabase
{
	public interface ISearchable
	{
		bool Hidden { get; set; }
		string Keywords { get; set; }
		string GenerateKeywords();
	}
}
