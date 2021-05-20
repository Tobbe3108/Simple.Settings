using System.IO;
using System.Security.Cryptography;

namespace Simple.Settings.Helpers
{
  public static class EncryptionHelper
  {
    private static readonly byte[] Salt = {10, 20, 30, 40, 50, 60, 70, 80};

    public static void Encrypt(this string toWrite, string? privateKey, FileInfo fileInfo)
    {
      using FileStream fileStream = new(fileInfo.Name, FileMode.Open);
      using Aes aes = Aes.Create();
      aes.Key = CreateKey(privateKey);
      byte[] iv = aes.IV;
      fileStream.Write(iv, 0, iv.Length);
      using CryptoStream cryptoStream = new(
        fileStream,
        aes.CreateEncryptor(),
        CryptoStreamMode.Write);
      using StreamWriter encryptWriter = new(cryptoStream);
      encryptWriter.Write(toWrite);
    }

    public static string Decrypt(string? privateKey, FileInfo fileInfo)
    {
      using FileStream fileStream = new(fileInfo.Name, FileMode.Open);
      using Aes aes = Aes.Create();
      byte[] iv = new byte[aes.IV.Length];
      var numBytesToRead = aes.IV.Length;
      var numBytesRead = 0;
      while (numBytesToRead > 0)
      {
        var n = fileStream.Read(iv, numBytesRead, numBytesToRead);
        if (n == 0) break;

        numBytesRead += n;
        numBytesToRead -= n;
      }
      
      using CryptoStream cryptoStream = new(
        fileStream,
        aes.CreateDecryptor(CreateKey(privateKey), iv),
        CryptoStreamMode.Read);
      using StreamReader decryptReader = new(cryptoStream);
      return decryptReader.ReadToEnd();
    }

    private static byte[] CreateKey(string? password, int keyBytes = 32)
    {
      const int iterations = 300;
      var keyGenerator = new Rfc2898DeriveBytes(password, Salt, iterations);
      return keyGenerator.GetBytes(keyBytes);
    }
  }
}