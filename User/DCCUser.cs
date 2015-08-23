using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace DCCDatabase.User
{
	public interface IDCCUser : IData
	{
		string Email { get; set; }
		string Name { get; set; }
		string USCFNumber { get; set; }
		bool UnclaimedAccount { get; set; }
		DateTime? Expiration { get; set; }
		Entitlement Entitlements { get; set; }
		DateTime? LastLogon { get; set; }
		bool AllowEmails { get; set; }
	}

	[Flags]
	public enum Entitlement
	{
		Guest = 0,
		/// <summary>This user has verified their email address
		/// </summary>
		Verified = 1 << 0,

		/// <summary>This user has administrative access to the site and the CMS
		/// </summary>
		Administrator = 1 << 7
	}

	public class DCCUser : BaseDataModel, IDCCUser
	{
		[Required(ErrorMessage = "Please enter an email address")]
		[DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
		[Display(Name = "Email Address", Description = "Your email will be used to identify you when you log in or recover your account. Emails will not be shared with third parties and will not be listed publically.")]
		public string Email { get; set; }

		[StringLength(64, ErrorMessage = "Cannot be more than 64 characters")]
		[Required(ErrorMessage = "Please enter your name")]
		public string Name { get; set; }

		public string ConfirmationCode { get; set; }

		[Display(Name = "USCF #", Description = "USCF numbers are used to determine the status of users with the United States Chess Federation. USCF membership is required to participate in rated events.")]
		public string USCFNumber { get; set; }

		/// <summary>Indicates the user must reset their password after login
		/// </summary>
		public bool ResetPassword { get; set; }

		/// <summary>Indicates a generated account is not yet claimed
		/// </summary>
		public bool UnclaimedAccount { get; set; }

		/// <summary>Date the user's dcc membership expires
		/// </summary>
		[Display(Name = "Expiration Date", Description = "When should this person's membership expire?")]
		public DateTime? Expiration { get; set; }

		public string Status { get { return MembershipStatus(); } }

		public bool IsMember { get { return Expiration > DateTime.Now; } }

		private string MembershipStatus()
		{
			return Expiration.HasValue ? (Expiration > DateTime.UtcNow ? "Current" : "Expired") : "Non-member";
		}

		/// <summary>salted hash of user's password
		/// </summary>
		public byte[] PasswordHash { get; set; }

		/// <summary>user type (for entitlements)
		/// </summary>
		public int UserType { get; set; }

		public DateTime? LastLogon { get; set; }

		[NotMapped]
		public Entitlement Entitlements { get { return (Entitlement)UserType; } set { UserType = (int)value; } }

		/// <summary>Allows the DCC to send emails to users regarding upcoming events, news, and other important information
		/// </summary>
		public bool AllowEmails { get; set; }

		private Guid? _salt;

		public Guid? Salt
		{
			get { return _salt = _salt ?? Guid.NewGuid(); }
			set { _salt = value; }
		}

		/// <summary>Check if the password is correct
		/// </summary>
		/// <param name="pass">the plain text password</param>
		/// <returns>true if the passwords match</returns>
		public bool CheckPassword(string pass)
		{
			// make sure the salt has value
			if (Salt.HasValue)
			{
				// compare the bytes of the password hash (stored in database)
				// and the provided string run through the same hashing algorithm
				return CompareByteArrays(PasswordHash, GenerateSaltedHash(pass, Salt));
			}

			return false;
		}

		/// <summary>Convert plain text and salt into a hashed set of bytes
		/// </summary>
		/// <param name="rawPassword">the text to convert</param>
		/// <param name="salt">the salt</param>
		/// <returns>A set of bytes corresponding to the text and salt</returns>
		public static byte[] GenerateSaltedHash(string rawPassword, Guid? salt)
		{
			// make sure the plain text password is not null or empty
			// and make sure the salt has a value
			if (String.IsNullOrEmpty(rawPassword) || !salt.HasValue)
			{
				return null;
			}

			// get the bytes of the password and the salt
			var passwordBytes = Encoding.UTF8.GetBytes(rawPassword);
			var saltBytes = Encoding.UTF8.GetBytes(salt.Value.ToString("N"));

			// use SHA256 to hash the password
			HashAlgorithm algorithm = new SHA256Managed();

			// the byte array to be returned of set length
			byte[] plainTextWithSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];

			// loop over all of the plain text bytes
			// and add them to the output
			for (int i = 0; i < passwordBytes.Length; i++)
			{
				plainTextWithSaltBytes[i] = passwordBytes[i];
			}

			// do the same for the salt
			for (int i = 0; i < saltBytes.Length; i++)
			{
				plainTextWithSaltBytes[rawPassword.Length + i] = saltBytes[i];
			}

			// run the hashing algorithm twice for extra security
			return algorithm.ComputeHash(algorithm.ComputeHash(plainTextWithSaltBytes));
		}

		/// <summary>Compare the bytes of two arrays to verify their equality
		/// </summary>
		/// <param name="array1">The first set of bytes</param>
		/// <param name="array2">The second set of bytes (is compared against array1)</param>
		/// <returns>true if match</returns>
		private static bool CompareByteArrays(byte[] array1, byte[] array2)
		{
			if (array1.Length != array2.Length)
			{
				return false;
			}

			for (int i = 0; i < array1.Length; i++)
			{
				if (array1[i] != array2[i])
				{
					// return false if a single byte is incorrect
					return false;
				}
			}

			return true;
		}


		public bool IsEntitled(Entitlement entitlement)
		{
			return (Entitlements & entitlement) == entitlement;
		}
	}

	public class UserSearchOutput : IDCCUser
	{
		public int? TotalResults { get; set; }
		public int? ID { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string USCFNumber { get; set; }
		public bool UnclaimedAccount { get; set; }
		public DateTime? Expiration { get; set; }
		public Entitlement Entitlements { get { return (Entitlement)UserType; } set { UserType = (int)value; } }
		public DateTime? LastLogon { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? Modified { get; set; }
		public DCCUser ModifiedBy { get; set; }
		public bool AllowEmails { get; set; }
		public int? UserType { get; set; }
	}
}
