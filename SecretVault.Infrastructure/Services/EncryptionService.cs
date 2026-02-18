using System.Text;
using System.Security.Cryptography;
using SecretVault.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using SecretVault.Application.Interfaces;

namespace SecretVault.Infrastructure.Services;

public class EncryptionService : IEncryptionService
{
    private const string EncryptionKeyName = "EncryptionKey:SecretKey";
    private readonly byte[] _key;

    public EncryptionService(IConfiguration configuration)
    {
        var secretKey = configuration[EncryptionKeyName];
        Guard.AgainstNullOrWhiteSpace(secretKey, EncryptionKeyName);
        _key = SHA256.HashData(Encoding.UTF8.GetBytes(secretKey!));
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            return plainText;
        }

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV();
        var iv = aes.IV;
        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        using var memoryStream = new MemoryStream();
        memoryStream.Write(iv, 0, iv.Length);
        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        {
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(plainText);
            }
        }
        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
        {
            return cipherText;
        }

        var fullCipher = Convert.FromBase64String(cipherText);
        using var aes = Aes.Create();
        var iv = new byte[aes.BlockSize / 8];
        Array.Copy(fullCipher, 0, iv, 0, iv.Length);
        aes.IV = iv;
        aes.Key = _key;

        using var decryptor = aes.CreateDecryptor(aes.Key, iv);
        using var memoryStream = new MemoryStream();
        memoryStream.Write(fullCipher, iv.Length, fullCipher.Length - iv.Length);
        memoryStream.Seek(0, SeekOrigin.Begin);

        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        return streamReader.ReadToEnd();
    }
}