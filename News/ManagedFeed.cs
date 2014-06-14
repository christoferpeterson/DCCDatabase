using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DCCDatabase.News
{
	public class ManagedFeed : BaseDataModel
	{
		/// <summary>The display name of this feed
		/// </summary>
		[Required]
		[StringLength(128, MinimumLength=5)]
		[Display(Name = "Name", Description = "The friendly, display name of this feed.")]
		public string FeedName { get; set; }

		/// <summary>The url of the rss feed
		/// </summary>
		[Required]
		[Display(Name = "URL", Description = "The url of the feed. A head request will be sent to verify existence.")]
		public string URL { get; set; }

		/// <summary>When was this feed last polled?
		/// </summary>
		public DateTime? LastPolled { get; set; }

		/// <summary>Determines how often the feed should be polled
		/// </summary>
		[Required]
		[DefaultValue(15)]
		[UIHint("Number")]
		[Range(15, 1440)]
		[Display(Name = "Poll Rate (in minutes)", Description = "Set how often this RSS feed gets polled in increments of 15 minimum is 15, maximum 1440 (1 day). Values will be rounded up to the nearest quarter hour." )]
		public int? PollRate { get; set; }

		/// <summary>Mark whether this feed is actively being polled
		/// </summary>
		[DefaultValue(true)]
		[Display(Name = "Active", Description = "While active, this feed will be polled on regular intervals.")]
		[UIHint("Checkbox")]
		public bool? Active { get; set; }

		/// <summary>The stored RSS Feed details
		/// </summary>
		public RSSFeed RssFeed { get; set; }

		/// <summary>The default managed feed item
		/// </summary>
		public static ManagedFeed Default { get { return new ManagedFeed { PollRate = 15, Active = true }; } }
	}
}
