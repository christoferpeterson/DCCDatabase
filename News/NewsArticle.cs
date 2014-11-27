using DCCDatabase;
using DCCDatabase.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCCDatabase.News
{
	public class NewsArticle : BaseDataModel
	{
		[NotMapped]
		[UIHint("ImageUpload")]
		[Display(Name = "Image", Description = "Add a jpg or png image that is 250 kb or less")]
		public HttpPostedFileBase Image { get; set; }

		/// <summary>Image file name
		/// </summary>
		public string ImageName { get; set; }

		[Display(Name = "Headline", Description = "Enter a descriptive, informative headline for the article.")]
		[Required(ErrorMessage = "Please provide a headline for this news article.")]
		[StringLength(64, MinimumLength = 5, ErrorMessage = "News headlines should be between 5 and 64 characters.")]
		public string Headline { get; set; }

		[AllowHtml]
		[Display(Name = "Text", Description = "Place the text of the news article here. Markdown is turned on. View the mark down helper for more information.")]
		[Required(ErrorMessage = "Please provide some body text for this news article.")]
		[DataType(DataType.MultilineText)]
		public string MarkdownText { get; set; }

		[Display(Name = "Teaser", Description = "A synopsis of the article to give a brief overview of content and entice readers.")]
		[Required(ErrorMessage = "Provide a quick synopsis of the article to entice readers.")]
		[StringLength(255, MinimumLength = 5, ErrorMessage = "News teasers should be between 5 and 255 characters.")]
		[DataType(DataType.MultilineText)]
		public string Teaser { get; set; }

		/// <summary>The raw text of the article (no markdown or html)
		/// </summary>
		public string RawText { get; set; }

		public Status State { get { return Status.Published; } }

		/// <summary>Transformed markdown text
		/// </summary>
		[AllowHtml]
		public string HtmlText { get; set; }

		public DCCUser Author { get; set; }

		public int Views { get; set; }

		[Flags]
		public enum Status
		{
			Pending,
			Published
		}
	}
}
