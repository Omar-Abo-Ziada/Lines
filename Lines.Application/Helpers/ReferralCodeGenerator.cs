using System.Security.Cryptography;

public static class ReferralCodeGenerator
{
    private const string Chars = "ABCDEFGHJKLMNPQRSTUVWXYZ123456789";
    private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

    public static string Generate(int length = 8)
    {
        var bytes = new byte[length];
        Rng.GetBytes(bytes);
        var result = new char[length];
        for (int i = 0; i < length; i++)
            result[i] = Chars[bytes[i] % Chars.Length];
        return new string(result);
    }
}
