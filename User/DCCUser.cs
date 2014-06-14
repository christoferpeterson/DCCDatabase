using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace DCCDatabase.User
{
	public class DCCUser : BaseDataModel
	{
		[Required(ErrorMessage = "Please enter an email address")]
		[DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
		[Display(Name = "Email Address", Description = "Your email will be used to identify you when you log in or recover your account. Emails will not be shared with third parties.")]
		public string Email { get; set; }

		[StringLength(64, ErrorMessage = "Cannot be more than 64 characters")]
		[Required(ErrorMessage = "Please enter your name")]
		public string Name { get; set; }

		public byte[] PasswordHash { get; set; }
		public Guid? Salt { get; set; }

		public bool CheckPassword(string pass)
		{
			if (Salt.HasValue)
			{
				return CompareByteArrays(PasswordHash, GenerateSaltedHash(pass, Salt.Value.ToString("N")));
			}

			return false;
		}

		private byte[] GenerateSaltedHash(string plainText, string salt)
		{
			if (String.IsNullOrEmpty(plainText))
			{
				return null;
			}

			var passwordBytes = Encoding.UTF8.GetBytes(plainText);
			var saltBytes = Encoding.UTF8.GetBytes(salt);

			HashAlgorithm algorithm = new SHA256Managed();

			byte[] plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

			for (int i = 0; i < plainText.Length; i++)
			{
				plainTextWithSaltBytes[i] = passwordBytes[i];
			}
			for (int i = 0; i < salt.Length; i++)
			{
				plainTextWithSaltBytes[plainText.Length + i] = saltBytes[i];
			}

			return algorithm.ComputeHash(algorithm.ComputeHash(plainTextWithSaltBytes));
		}

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
					return false;
				}
			}

			return true;
		}
	}
}
