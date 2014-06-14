using DCCDatabase.User;
using System;
using System.ComponentModel.DataAnnotations;

namespace DCCDatabase
{
	public abstract class BaseDataModel
	{
		/// <summary>The unique ID for this item
		/// </summary>
		[Key]
		public int? ID { get; set; }

		/// <summary>The UTC time this entry was generated
		/// </summary>
		public DateTime? Created { get; set; }

		/// <summary>The UTC time this entry was last modified
		/// </summary>
		public DateTime? Modified { get; set; }

		/// <summary>User that last modified this item
		/// </summary>
		public DCCUser ModifiedBy { get; set; }
	}
}
