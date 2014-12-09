using DCCDatabase.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DCCDatabase.Store
{
	public class Transaction : BaseDataModel
	{
		private static double rnExponent = 2.732;
		private static int rnStart = new Random().Next(0, 1000);

		/// <summary>Which user made this transaction?
		/// </summary>
		public DCCUser User { get; set; }

		/// <summary>The name on the card
		/// </summary>
		[Required]
		public string NameOnCard { get; set; }

		[Required]
		public string StripeToken { get; set; }

		/// <summary>The total cost of the transaction
		/// </summary>
		[Required]
		public double Total { get; set; }

		private int? _receiptNumber;

		/// <summary>To be calculated. Math.Ceiling(( + id)^2.7);
		/// </summary>
		public int ReceiptNumber
		{
			get { _receiptNumber = _receiptNumber ?? (int)(Created.Value.ToOADate() * 1000 + Math.Ceiling(Math.Pow(rnStart, rnExponent))); return _receiptNumber.Value; }
			set { _receiptNumber = value; }
		}

		[DataType(DataType.EmailAddress)]
		[Required]
		public string Email { get; set; }

		/// <summary>The shipping address of the customer (only necessary if something is being delivered)
		/// </summary>
		public string ShippingAddress { get; set; }

		/// <summary>The billing address of the customer (for cc verification and auditing)
		/// </summary>
		[Required]
		public string BillingAddress { get; set; }

		public ICollection<TransactionItem> Items { get; set; }
	}
}
