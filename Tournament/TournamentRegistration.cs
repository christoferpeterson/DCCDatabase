namespace DCCDatabase.Tournament
{
	public class OldTournamentRegistration : BaseDataModel
	{
		public string USCFID { get; set; }
		public string Name { get; set; }
		public int? Rating { get; set; }
		public string Tournament { get; set; }
		public string Section { get; set; }
		public string ReceiptNumber { get; set; }
	}
}
