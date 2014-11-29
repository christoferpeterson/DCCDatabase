namespace DCCDatabase
{
	public interface ISearchable
	{
		bool Hidden { get; }
		string Keywords { get; set; }
		string GenerateKeywords();
	}
}
