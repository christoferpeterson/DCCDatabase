using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DCCDatabase.Store
{
	/// <summary>Interface that outlines how a store service should be put together
	/// </summary>
	public interface IStoreService
	{
		bool Success { get; }
		void Prepare();
		void Init();
		void Load();

		/// <summary>Serialized collection of service parameters
		/// </summary>
		/// <returns>json string</returns>
		string ServiceParameters();

		/// <summary>Serialized collection of user inputs
		/// </summary>
		/// <returns>json string</returns>
		string UserInputs();
	}

	public class StoreProduct : BaseDataModel, ISearchable
	{
		public Guid Guid { get; set; }

		public ShortGuid ShortGuid { get { return (ShortGuid)Guid; } }


		/// <summary>True = published, false = pending, null = deleted
		/// </summary>
		[UIHint("Checkbox")]
		[Display(Name = "Published", Description = "Publish this product in the store.")]
		public bool Published { get; set; }

		/// <summary>Uploaded file of the image
		/// </summary>
		[NotMapped]
		[UIHint("ImageUpload")]
		[Display(Name = "Image", Description = "Add a jpg or png image that is 250 kb or less")]
		public HttpPostedFileBase Image { get; set; }

		/// <summary>Image file name
		/// </summary>
		public string ImageName { get; set; }

		[Required(ErrorMessage = "Please provide a category name.")]
		[StringLength(64, MinimumLength = 3, ErrorMessage = "Item names must be between 3 and 64 characters in length.")]
		[Display(Name = "Name", Description = "Enter a name for this product.", Prompt = "Purple Chess Board")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please describe this category.")]
		[DataType(DataType.MultilineText)]
		[Display(
			Name = "Description", 
			Description = "Place the text of the news article here. Markdown is turned on. View the mark down helper for more information.",
			Prompt = "Use markdown to write up a rich text description of this product or service."
		)]
		public string Description { get; set; }

		/// <summary>Transformed markdown text
		/// </summary>
		[AllowHtml]
		public string HtmlText { get; set; }
		
		/// <summary>The price of the item
		/// </summary>
		[Display(Name = "Price", Description = "What is the price of this product or service?", Prompt = "9.99")]
		[DataType(DataType.Currency)]
		public double? Price { get; set; }

		[Display(Name = "Expiry Date", Description = "When does this product no longer become available?")]
		public DateTime? Expire { get; set; }

		/// <summary>Restrict this item to DCC members only
		/// </summary>
		[Display(Name = "Members Only", Description = "Make this item available to DCC members only.")]
		[DataType("Checkbox")]
		public bool DCCMembersOnly { get; set; }

		[Display(Name = "User Must be Logged In", Description = "This item requires the user to be logged into their account.")]
		[DataType("Checkbox")]
		public bool LoggedInOnly { get; set; }

		[Display(Name = "Requires Address", Description = "Check this box if the item needs to be shipped to the customer.")]
		[DataType("Checkbox")]
		public bool RequiresAddress { get; set; }

		/// <summary>The service to run when this item gets processed
		/// <see cref="StoreService.cs"/>
		/// </summary>
		public string Service { get; set; }

		/// <summary>Additional information about the service, json serialized
		/// </summary>
		public string ServiceParameters { get; set; }

		[NotMapped]
		public IStoreService ServiceDetails { get; set; }

		private string _keywords;
		public string Keywords
		{
			get { _keywords = _keywords ?? GenerateKeywords(); return _keywords; }
			set { _keywords = value; }
		}

		public string GenerateKeywords()
		{
			var words = Regex.Split(Description, @"\W+") // get all words
				.Where(s => s.Length > 3) // exclude short words
				.GroupBy(s => s) // group by each word
				.OrderByDescending(g => g.Count()) // sort by the number of that word
				.Select(g => g.First()); // grab the word

			var keywords = words.Except(DatabaseConfiguration.CommonWords) // exclude common words
								.Take(10) // take the top 10
								.ToList(); // convert to a list
			return String.Join(",", keywords); // comma delimit
		}
	}
}
