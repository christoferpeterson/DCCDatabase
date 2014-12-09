using DCCDatabase;
using DCCDatabase.User;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using MarkdownSharp;

namespace DCCDatabase.News
{
	public class NewsArticle : BaseDataModel, ISearchable
	{
		[Flags]
		public enum Status
		{
			Deleted = 1 << 0,
			Pending = 1 << 1,
			Published = 1 << 2,

			Hidden = Deleted | Pending
		}

		/// <summary>Uploaded file of the image
		/// </summary>
		[NotMapped]
		[UIHint("ImageUpload")]
		[Display(Name = "Image", Description = "Add a jpg or png image that is 250 kb or less")]
		public HttpPostedFileBase Image { get; set; }

		/// <summary>Image file name
		/// </summary>
		public string ImageName { get; set; }

		/// <summary>The headline of the article
		/// </summary>
		[Display(Name = "Headline", Description = "Enter a descriptive, informative headline for the article.")]
		[Required(ErrorMessage = "Please provide a headline for this news article.")]
		[StringLength(64, MinimumLength = 5, ErrorMessage = "News headlines should be between 5 and 64 characters.")]
		public string Headline { get; set; }

		/// <summary>The raw text of the news article including markdown
		/// </summary>
		[AllowHtml]
		[Display(Name = "Text", Description = "Place the text of the news article here. Markdown is turned on. View the mark down helper for more information.")]
		[Required(ErrorMessage = "Please provide some body text for this news article.")]
		[DataType(DataType.MultilineText)]
		public string MarkdownText { get; set; }

		/// <summary>Article synopsis
		/// </summary>
		[Display(Name = "Teaser", Description = "A synopsis of the article to give a brief overview of content and entice readers.")]
		[Required(ErrorMessage = "Provide a quick synopsis of the article to entice readers.")]
		[StringLength(255, MinimumLength = 5, ErrorMessage = "News teasers should be between 5 and 255 characters.")]
		[DataType(DataType.MultilineText)]
		public string Teaser { get; set; }

		/// <summary>The raw text of the article (no markdown or html)
		/// </summary>
		public string RawText { get; set; }

		/// <summary>Current state of the article
		/// </summary>
		public Status State { get { return Status.Published; } }

		/// <summary>Transformed markdown text
		/// </summary>
		[AllowHtml]
		public string HtmlText { get; set; }

		/// <summary>Author of the article
		/// </summary>
		public DCCUser Author { get; set; }

		/// <summary>Number of fews this article has
		/// </summary>
		public int Views { get; set; }

		/// <summary>Determines if the article should be hidden from searches
		/// </summary>
		[NotMapped]
		public bool Hidden { get { return State.HasFlag(Status.Hidden); } }

		/// <summary>Keywords as calcualted from the article's most used words
		/// </summary>
		[Display(Name = "Keywords", Description="A comma delimited list of key words or phrases for searching.")]
		public string Keywords
		{
			get { _keywords = _keywords ?? GenerateKeywords(); return _keywords; }
			set { _keywords = value; }
		}
		private string _keywords;

		/// <summary>Gets a comma delimited list of keywords by getting the most common words in the content
		/// common words (Config.CommonWords) and words less than 3 characters long are excluded
		/// </summary>
		public string GenerateKeywords()
		{
			// transform the content into something useable:
			//	-remove markdown and html
			//	-convert to all lowercase
			var content = new Markdown().Transform(MarkdownText).StripHtml().ToLower();
			var words = Regex.Split(content, @"\W+") // get all words
				.Where(s => s.Length > 3) // exclude short words
				.GroupBy(s => s) // group by each word
				.OrderByDescending(g => g.Count()) // sort by the number of that word
				.Select(g => g.First()); // grab the word

			var keywords = words.Except(CommonWords) // exclude common words
								.Take(10) // take the top 10
								.ToList(); // convert to a list
			return String.Join(", ", keywords); // comma delimit
		}
	}
}
