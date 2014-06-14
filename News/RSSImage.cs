using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCCDatabase.News
{
	/// <summary>http://validator.w3.org/feed/docs/rss2.html#ltimagegtSubelementOfLtchannelgt
	/// </summary>
	public class RSSImage : BaseDataModel
	{
		/// <summary>URL of a GIF, JPEG or PNG image that represents the channel or item
		/// </summary>
		public string URL { get; set; }

		/// <summary>describes the image, it's used in the ALT attribute of the HTML img tag when the channel is rendered in HTML
		/// </summary>
		public string Title { get; set;}

		/// <summary>URL of the site, when the channel is rendered, the image is a link to the site. (Note, in practice the image 
		/// title and link should have the same value as the channel's title and link
		/// </summary>
		public string Link { get; set; }

		/// <summary>description contains text that is included in the TITLE attribute of the link formed around the image in the HTML rendering.
		/// </summary>
		public string Description { get; set; }

		/// <summary>Optional elements include width and height, numbers, indicating the width and height of the image in pixels.
		/// </summary>
		[Range(0, 144)]
		public int? Width { get; set; }

		/// <summary>Optional elements include width and height, numbers, indicating the width and height of the image in pixels.
		/// </summary>
		[Range(0, 400)]
		public int? Height { get; set; }
	}
}
