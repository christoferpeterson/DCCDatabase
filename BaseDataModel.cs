using DCCDatabase.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCCDatabase
{
	public interface IData
	{
		int? ID { get; set; }
		DateTime? Created { get; set; }
		DateTime? Modified { get; set; }
		DCCUser ModifiedBy { get; set; }
	}

	public abstract class BaseDataModel : IData
	{
		/// <summary>The unique ID for this item
		/// </summary>
		[Key]
		public int? ID { get; set; }

		/// <summary>The UTC time this entry was generated
		/// </summary>
		public DateTime? Created { get; set; }

		/// <summary>The UTC time this entry was last modified
		/// </summary>
		public DateTime? Modified { get; set; }

		/// <summary>User that last modified this item
		/// </summary>
		public DCCUser ModifiedBy { get; set; }

		/// <summary>A list of common words to exclude when programatically determining keywords
		/// </summary>
		protected readonly string[] CommonWords = 
		{ 
			"the","be","to","of","and","a","in","that","have","I","it","for","not","on","with","he","as","you","do","at","this","but","his",
			"by","from","they","we","say","her","she","or","an","will","my","one","all","would","there","their","what","so","up","out","if","about",
			"who","get","which","go","me","when","were","make","can","like","time","no","just","him","know","take","people","into","year","your","good","some",
			"could","them","see","other","than","then","now","look","only","come","its","over","think","also","back","after","use","two","how","our",
			"work","first","well","way","even","new","want","because","any","these","give","day","most","us"
		};
	}
}
