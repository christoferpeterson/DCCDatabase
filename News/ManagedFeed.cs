using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DCCDatabase.News
{
	public class ManagedFeed : BaseDataModel
	{
		public const int DefaultPollRate = 15;

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
		[UIHint("UI/Timestamp")]
		public DateTime? LastPolled { get; set; }

		/// <summary>Determines how often the feed should be polled
		/// </summary>
		[Required]
		[DefaultValue(DefaultPollRate)]
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

		/// <summary>The name of the channel. It's how people refer to your service. If you have an HTML website
		/// hat contains the same information as your RSS file, the title of your channel should be the same as the 
		/// title of your website.
		/// </summary>
		public string Title { get; set; }

		/// <summary>The URL to the HTML website corresponding to the channel.
		/// </summary>
		public string Link { get; set; }

		/// <summary>Phrase or sentence describing the channel.
		/// </summary>
		public string Description { get; set; }

		/// <summary>The language the channel is written in. This allows aggregators to group all Italian language sites,
		/// for example, on a single page. A list of allowable values for this element, as provided by Netscape, is here (http://backend.userland.com/stories/storyReader$16).
		/// You may also use values defined by the W3C (http://www.w3.org/TR/REC-html40/struct/dirlang.html#langcodes).
		/// </summary>
		public string Language { get; set; }

		/// <summary>Copyright notice for content in the channel.
		/// </summary>
		public string Copyright { get; set; }

		/// <summary>Email address for person responsible for editorial content.
		/// </summary>
		public string ManagingEditor { get; set; }

		/// <summary>Email address for person responsible for technical issues relating to channel.
		/// </summary>
		public string Webmaster { get; set; }

		/// <summary>The publication date for the content in the channel. For example, the New York Times 
		/// publishes on a daily basis, the publication date flips once every 24 hours. That's when the 
		/// pubDate of the channel changes. All date-times in RSS conform to the Date and Time Specification 
		/// of RFC 822 (http://asg.web.cmu.edu/rfc/rfc822.html), with the exception that the year may be 
		/// expressed with two characters or four characters (four preferred).
		/// </summary>
		public DateTime? PublishDate { get; set; }

		/// <summary>The last time the content of the channel changed.
		/// </summary>
		public DateTime? LastBuildDate { get; set; }

		/// <summary>Specify one or more categories that the channel belongs to. Follows the same rules as the 
		/// item-level category element. http://validator.w3.org/feed/docs/rss2.html#syndic8
		/// </summary>
		public string Category { get; set; }

		/// <summary>A string indicating the program used to generate the channel.
		/// </summary>
		public string Generator { get; set; }

		/// <summary>A URL that points to the documentation for the format used in the RSS file. It's probably a pointer 
		/// to this page. It's for people who might stumble across an RSS file on a Web server 25 years from now and wonder 
		/// what it is.
		/// </summary>
		public string Docs { get; set; }

		/// <summary>Allows processes to register with a cloud to be notified of updates to the channel, implementing a 
		/// lightweight publish-subscribe protocol for RSS feeds. More info: http://validator.w3.org/feed/docs/rss2.html#ltcloudgtSubelementOfLtchannelgt
		/// </summary>
		public string Cloud { get; set; }

		/// <summary>ttl stands for time to live. It's a number of minutes that indicates how long a channel can be 
		/// cached before refreshing from the source. More info: http://validator.w3.org/feed/docs/rss2.html#ltttlgtSubelementOfLtchannelgt
		/// </summary>
		public int? TimeToLive { get; set; }

		/// <summary>Specifies a GIF, JPEG or PNG image that can be displayed with the channel. More info here:
		/// http://validator.w3.org/feed/docs/rss2.html#ltimagegtSubelementOfLtchannelgt
		/// </summary>
		public string Image { get; set; }

		/// <summary>Specifies a text input box that can be displayed with the channel. More info here:
		/// http://validator.w3.org/feed/docs/rss2.html#lttextinputgtSubelementOfLtchannelgt
		/// </summary>
		public string textInput { get; set; }

		/// <summary>A hint for aggregators telling them which hours they can skip. More info here:
		/// http://backend.userland.com/skipHoursDays#skiphours
		/// </summary>
		public int? SkipHours { get; set; }

		/// <summary>A hint for aggregators telling them which days they can skip. More info here:
		/// http://backend.userland.com/skipHoursDays#skipdays
		/// </summary>
		public int? SkipDays { get; set; }

		/// <summary>A collection of RSS Feed Items that were broadcast over this feed. More info:
		/// http://validator.w3.org/feed/docs/rss2.html#hrelementsOfLtitemgt
		/// </summary>
		public List<RSSFeedItem> Items { get; set; }

		/// <summary>The default managed feed item
		/// </summary>
		public static ManagedFeed Default { get { return new ManagedFeed { PollRate = 15, Active = true }; } }

		public DateTime NextPoll { get { return LastPolled.HasValue ? LastPolled.Value.AddMinutes(PollRate ?? DefaultPollRate) : DateTime.MinValue; } }
	}
}
