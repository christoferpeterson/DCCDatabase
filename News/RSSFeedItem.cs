﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DCCDatabase.News
{
	/// <summary>http://validator.w3.org/feed/docs/rss2.html#hrelementsOfLtitemgt
	/// </summary>
	public class RSSFeedItem : BaseDataModel
	{
		/// <summary>The title of the item.
		/// </summary>
		public string Title { get; set; }

		/// <summary>The URL of the item.
		/// </summary>
		public string Link { get; set; }

		/// <summary>The item synopsis. http://validator.w3.org/feed/docs/rss2.html#ltauthorgtSubelementOfLtitemgt
		/// </summary>
		public string Description { get; set; }

		/// <summary>Email address of the author of the item. http://validator.w3.org/feed/docs/rss2.html#ltauthorgtSubelementOfLtitemgt
		/// </summary>
		public string Author { get; set; }

		/// <summary>URL of a page for comments relating to the item. http://validator.w3.org/feed/docs/rss2.html#ltcommentsgtSubelementOfLtitemgt
		/// </summary>
		public string Comments { get; set; }

		/// <summary>A string that uniquely identifies the item. http://validator.w3.org/feed/docs/rss2.html#ltguidgtSubelementOfLtitemgt
		/// </summary>
		public Guid? Guid { get; set; }

		/// <summary>A unique identifier if the Guid doesn't exist or is malformatted
		/// </summary>
		public string UniqueID { get; set; }

		/// <summary>Indicates when the item was published. http://validator.w3.org/feed/docs/rss2.html#ltpubdategtSubelementOfLtitemgt
		/// </summary>
		public DateTime? PublishDate { get; set; }

		/// <summary>The timezone adjusted published date
		/// </summary>
		[NotMapped]
		public DateTime? PublishDateAdjusted { get; set; }

		/// <summary>The RSS channel that the item came from. http://validator.w3.org/feed/docs/rss2.html#ltsourcegtSubelementOfLtitemgt
		/// </summary>
		public string Source { get; set; }

		/// <summary>The source feed
		/// </summary>
		[ScriptIgnore]
		public ManagedFeed Feed { get; set; }
	}
}
