using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCCDatabase.User
{
	public class UserProfile : BaseDataModel
	{
		/// <summary>DCCUser ID associated with this account
		/// </summary>
		public int? UserID { get; set; }

		[Display(Name = "Street One", Description = "Provide an address so the DCC can send prize checks and other important information. Addresses are not listed publically nor shared with third parties.")]
		public string StreetOne { get; set; }
		[Display(Name = "Street Two")]
		public string StreetTwo { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }

		[Display(Name = "Phone #", Description = "Provide the best phone number to contact you. Phone numbers are not shared with third parties nor displayed publically on the site.")]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[Display(Name = "Keep Profile Private", Description = "Mark your profile private so only you and administrators of the site can see it. Contact information is already private but this will keep your entire online profile from being viewed.")]
		[UIHint("Checkbox")]
		public bool? KeepPrivate { get; set; }

		public int Views { get; set; }
	}
}
