using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DCCDatabase.Store
{
	public class StoreProduct : BaseDataModel, ISearchable
	{
		[Required(ErrorMessage = "Please provide a category name.")]
		[StringLength(20, MinimumLength = 3, ErrorMessage = "Item names must be between 3 and 20 characters in length.")]
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

		[Display(Name = "Hide this category from searches")]
		public bool Hidden { get; set; }

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

		[Display(Name = "Requires Address", Description = "Check this box if the item needs to be shipped to the customer.")]
		[DataType("Checkbox")]
		public bool RequiresAddress { get; set; }

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
