using DCCDatabase.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DCCDatabase.Store
{
	public class Transaction : BaseDataModel
	{
		private static double rnExponent = 2.732;
		private static int rnStart = new Random().Next(0, 1000);

		[ForeignKey("User")]
		public int? User_ID { get; set; }

		/// <summary>Which user made this transaction?
		/// </summary>
		public DCCUser User { get; set; }

		/// <summary>The name on the card
		/// </summary>
		public string NameOnCard { get; set; }

		[Required]
		public string StripeToken { get; set; }

		/// <summary>The total cost of the transaction
		/// </summary>
		public double Total { get; set; }

		public double Subtotal { get; set; }
		public double TransactionFee { get; set; }
		public DateTime? Paid { get; set; }
		public string StripeID { get; set; }

		private int? _receiptNumber;

		/// <summary>To be calculated. Math.Ceiling(( + id)^2.7);
		/// </summary>
		public int ReceiptNumber
		{
			get { 
				_receiptNumber = _receiptNumber ?? 
				(int)(DateTime.UtcNow.ToOADate() * 10000 + Math.Ceiling(Math.Pow(rnStart, rnExponent))); 
				return _receiptNumber.Value; 
		}
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
		public string BillingAddress { get; set; }

		public ICollection<TransactionItem> Items { get; set; }
	}
}
