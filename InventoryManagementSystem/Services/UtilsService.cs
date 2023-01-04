using System.Security.Cryptography;

namespace InventoryManagementSystem.Services;

public class Utils
{
    private const char segment_Delimiter = ':';

    public static string HashSecret(string input)
    {
        var saltSize = 16;
        var iterations = 100_000;
        var keySize = 32;
        var algorithm = HashAlgorithmName.SHA256;
        var salt = RandomNumberGenerator.GetBytes(saltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

        var result = string.Join(
            segment_Delimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            iterations,
            algorithm
        );

        return result;
    }

    public static bool VerifyHash(string input, string hashString)
    {
        var segments = hashString.Split(segment_Delimiter);
        var hash = Convert.FromHexString(segments[0]);
        var salt = Convert.FromHexString(segments[1]);
        var iterations = int.Parse(segments[2]);
        var algorithm = new HashAlgorithmName(segments[3]);
        var inputHash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            iterations,
            algorithm,
            hash.Length
        );

        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }

    public static string Get_Directory_Path()
    {
        return @"E:\Inventory-Management-System";
    }

    public static string Get_Users_FilePath()
    {
        return Path.Combine(Get_Directory_Path(), "usersData.json");
    }

    public static string Get_Products_FilePath()
    {
        return Path.Combine(Get_Directory_Path(), "productsData.json");
    }

    public static string Get_Orders_FilePath()
    {
        return Path.Combine(Get_Directory_Path(), "ordersData.json");
    }
}
