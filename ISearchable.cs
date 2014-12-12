namespace DCCDatabase
{
	public interface ISearchable
	{
		bool Published { get; set; }
		string Keywords { get; set; }
		string GenerateKeywords();
	}
}
