using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCCDatabase.News
{
	public class ManagedFeed : BaseDataModel
	{
		public const int MIN_POLL_RATE = 60;
		public const int MAX_POLL_RATE = 1440;

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

		/// <summary>Adjust the poll date according to timezone
		/// </summary>
		[UIHint("UI/Timestamp")]
		[NotMapped]
		public DateTime? LastPolledAdjusted { get; set; }

		[Required]
		[Display(Name = "Region", Description = "Local, Colorado, National, or International")]
		public string Location { get; set; }

		/// <summary>Determines how often the feed should be polled
		/// </summary>
		[Required]
		[DefaultValue(MIN_POLL_RATE)]
		[UIHint("Number")]
		[Display(Name = "Poll Rate (in minutes)", Description = "Set how often this RSS feed gets polled in increments of 60, min: 60 max: 1440 (1 day).")]
		public int? PollRate 
		{ 
			get 
			{
				return _pollRate; 
			}
			set 
			{
				// keep the poll rate in range
				_pollRate = value.HasValue ? Math.Max(MIN_POLL_RATE, Math.Min(MAX_POLL_RATE, value.Value)) : MIN_POLL_RATE;
			}
		}
		private int _pollRate;

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
		[UIHint("UI/Timestamp")]
		public DateTime? PublishDate { get; set; }

		/// <summary>Set the publish date according to timezone
		/// </summary>
		[UIHint("UI/Timestamp")]
		[NotMapped]
		public DateTime? PublishDateAdjusted { get; set; }

		/// <summary>The last time the content of the channel changed.
		/// </summary>
		[UIHint("UI/Timestamp")]
		public DateTime? LastBuildDate { get; set; }

		/// <summary>Adjust the last build date according to timezone
		/// </summary>
		[UIHint("UI/Timestamp")]
		[NotMapped]
		public DateTime? LastBuildDateAdjusted { get; set; }

		private string _contentType;

		/// <summary>The content type of this rss feed
		/// see ManagedFeed.ContentTypes for examples
		/// </summary>
		public string ContentType
		{
			get 
			{
				// get the default content type if null
				_contentType = _contentType ?? ContentTypes[0];

				// verify the content type is supported
				if(!ContentTypes.Contains(_contentType))
				{
					// default to the first content type
					_contentType = ContentTypes[0];
				}
				return _contentType; 
			}
			set { _contentType = value; }
		}

		/// <summary>All the content types supported
		/// </summary>
		public static readonly string[] ContentTypes = new string[] { "article", "video", "tacticstime" };

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
		public static ManagedFeed Default { get { return new ManagedFeed { PollRate = MIN_POLL_RATE, Active = true }; } }

		public DateTime NextPoll { get { return LastPolled.HasValue ? LastPolled.Value.AddMinutes(PollRate ?? MIN_POLL_RATE) : DateTime.MinValue; } }

		/// <summary>flags whether the content of this feed is video
		/// </summary>
		public bool IsVideo { get { return ContentType == "video"; } }

		/// <summary>flags whether the content of this feed is from Tim Brennan's Tactics Time
		/// </summary>
		public bool HeadlineOnly { get { return IsVideo || ContentType == "tacticstime"; } }
	}
}
