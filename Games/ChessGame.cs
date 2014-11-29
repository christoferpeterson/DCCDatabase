using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCCDatabase.Games
{
	public class ChessGame : BaseDataModel
	{
		public enum ResultType
		{
			None,
			WhiteWins,
			BlackWins,
			Draw
		}

		/// <summary>ID in the old database
		/// </summary>
		public short OldID { get; set; }

		public string Event { get; set; }
		public string Site { get; set; }
		public DateTime? Date { get; set; }
		public string Round { get; set; }

		public string White { get; set; }
		public int? White_ELO { get; set; }
		public string White_USCF { get; set; }

		public string Black { get; set; }
		public int? Black_ELO { get; set; }
		public string Black_USCF { get; set; }

		public string ECO { get; set; }
		public ResultType Result { get; set; }
		public string Opening { get; set; }
		public string Variation { get; set; }

		public string PGN { get; set; }
		public bool Private { get; set; }

		public int Views { get; set; }
	}
}
