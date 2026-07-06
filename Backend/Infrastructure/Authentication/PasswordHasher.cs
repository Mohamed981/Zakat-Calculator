using System;
using System.Security.Cryptography;
using Application.Abstractions.Authentication;

namespace Infrastructure.Authentication;

internal sealed class PasswordHasher : IPasswordHasher
{
	private const int SaltSize = 16;

	private const int HashSize = 32;

	private const int Iterations = 500000;

	private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

	public string Hash(string password)
	{
		byte[] bytes = RandomNumberGenerator.GetBytes(16);
		byte[] inArray = Rfc2898DeriveBytes.Pbkdf2(password, bytes, 500000, Algorithm, 32);
		return Convert.ToHexString(inArray) + "-" + Convert.ToHexString(bytes);
	}

	public bool Verify(string password, string passwordHash)
	{
		string[] array = passwordHash.Split('-');
		byte[] array2 = Convert.FromHexString(array[0]);
		byte[] salt = Convert.FromHexString(array[1]);
		byte[] array3 = Rfc2898DeriveBytes.Pbkdf2(password, salt, 500000, Algorithm, 32);
		return CryptographicOperations.FixedTimeEquals(array2, array3);
	}
}
