using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;

namespace UsersAPI.Util
{
	public class HashingUtil
	{
		private const int _iterations = 2;
		private const int _memorySize = 65536;
		private const int _parallelism = 1;

		private static string GenerateSalt()
		{
			byte[] salt = new byte[16];

			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
				return Convert.ToBase64String(salt);
			}
		}

		public static string HashPassword(string password, out string salt)
		{
			salt = GenerateSalt();

			if (password == null) return "";

			return Hash(password, salt);
		}

		private static string Hash(string password, string salt, int parallelism = _parallelism, int iterations = _iterations, int memorySize = _memorySize)
		{
			using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(password)))
			{
				hasher.Salt = Encoding.UTF8.GetBytes(salt);
				hasher.DegreeOfParallelism = parallelism;
				hasher.Iterations = iterations;
				hasher.MemorySize = memorySize;

				return Convert.ToBase64String(hasher.GetBytes(32));
			}
		}

		public static bool Verify(string pass, string compareTo ,string salt)
		{
			string hash = Hash(pass, salt);

			return ConstantTimeCompare(Encoding.UTF8.GetBytes(hash), Encoding.UTF8.GetBytes(compareTo));
		}

		private static bool ConstantTimeCompare(byte[] array1, byte[] array2)
		{
			if (array1 == null || array2 == null || array1.Length != array2.Length)
			{
				return false;
			}

			int result = 0;
			for (int i = 0; i < array1.Length; i++)
			{
				result |= array1[i] ^ array2[i];
			}

			return result == 0;
		}

		static byte[] GetStoredHashedPassword()
		{
			throw new NotImplementedException();
		}

		static byte[] GetRetrievedSalt()
		{
			throw new NotImplementedException();
		}
	}
}
