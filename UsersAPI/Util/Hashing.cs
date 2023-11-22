using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;

namespace UsersAPI.Util
{
	public class Hashing
	{
		public static byte[] Salt()
		{
			byte[] salt = new byte[16];

			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
				return salt;
			}
		}

		public static byte[] Hash(string password, byte[] salt, int parallelism, int iterations, int memorySize)
		{
			using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(password)))
			{
				hasher.Salt = salt;
				hasher.DegreeOfParallelism = parallelism;
				hasher.Iterations = iterations;
				hasher.MemorySize = memorySize;

				return hasher.GetBytes(32);
			}
		}

		public static byte[] Hash(string password, byte[] salt)
		{
			int iterations = 2;
			int memorySize = 65536;
			int parallelism = 1;

			return Hash(password, salt, parallelism, iterations, memorySize);
		}

		static bool ConstantTimeCompare(byte[] array1, byte[] array2)
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
