using DCCDatabase.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DCCDatabase.Event
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

	public class EntryFees
	{
		[Key]
		public int? ID { get; set; }

		public int? Tournament_ID { get; set; }
		public int? StoreProduct_ID { get; set; }

		[ForeignKey("Tournament_ID")]
		public Tournament Tournament { get; set; }

		[ForeignKey("StoreProduct_ID")]
		public StoreProduct StoreProduct { get; set; }
	}

	public class Tournament : BaseDataModel
	{
		[Display(Name = "Event Name")]
		[Required(ErrorMessage = "Provide an event name.")]
		public string Name { get; set; }

		[Display(Name = "Start Date", Description="The date the tournament starts.")]
		public DateTime? StartDate { get; set; }

		[Display(Name = "End Date", Description = "The date the tournament ends.")]
		public DateTime? EndDate { get; set; }

		[Display(Name = "Tournament Details")]
		public string Description { get; set; }

		[Display(Name = "Publish Event", Description = "Mark this event as published and available for registration.")]
		public bool Published { get; set; }

		/// <summary>Pipe delimited list of sections?
		/// </summary>
		[Display(Name = "Sections", Description = "Provide a list of sections, separated by commas (e.g. Open, U1800, U1500).")]
		public string Sections { get; set; }

		public ICollection<TournamentRegistration> Registrations { get; set; }

		[NotMapped]
		public int? RegistrationCount { get { return Registrations.Count; } }

		public ICollection<EntryFees> EntryFees { get; set; }
	}

	public class TournamentRegistration : BaseDataModel
	{
		[ForeignKey("Tournament_ID")]
		public Tournament Tournament { get; set; }

		public int? Tournament_ID { get; set; }
		public string USCF { get; set; }
		public string Name { get; set; }
		public string Section { get; set; }
	}
}
