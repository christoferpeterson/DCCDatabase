using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace DCCDatabase.Store
{
	public class StoreCategory : BaseDataModel, ISearchable
	{
		[Required(ErrorMessage = "Please provide a category name.")]
		[StringLength(64, MinimumLength = 3, ErrorMessage = "Store categories must be between 3 and 20 characters in length.")]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please describe this category.")]
		[DataType(DataType.MultilineText)]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Display(Name = "Hide this category from searches")]
		[UIHint("Checkbox")]
		public bool Published { get; set; }

		public IEnumerable<StoreProduct> Products { get; set; }

		private string _keywords;
		public string Keywords
		{
			get { _keywords = _keywords ?? GenerateKeywords();  return _keywords; }
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
