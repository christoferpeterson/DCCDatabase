using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCCDatabase.Store
{
	public class TransactionItem : BaseDataModel
	{
		public StoreProduct Product { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		/// <summary>The number of items
		/// </summary>
		[Required]
		public int Quantity { get; set; }

		/// <summary>The price of this item when it was purchased. Until the item
		/// is purchased, this value is directly to tied to StoreProduct.CurrentPrice
		/// </summary>
		public double? Price { get; set; }
	}
}
