

using System.Security.Cryptography;
using System.Text;

/// <summary>
/// The main encryption logic. 
/// </summary>
public class AesGcmUtils
{
    // The length of the nonce (12 bytes)
    private static readonly int _nonceLen = 12;

    //  length of the authentication tag (16 bytes)
    private static readonly int _tagLen = 16;



    /// <param name="content">The data to encrypted</param>
    /// <param name="key">The 256-bit masterkey</param>
    /// <returns>Base64 string containing nonce + tag + ciphertext.</returns>
    /// <exception cref="Exception"></exception>
    public static string Encrypt(string content, byte[] key)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(content);
            ArgumentNullException.ThrowIfNull(key);

            if(key.Length != 32) throw new ArgumentException("Incorrect key size!");

            // Creates the nonce (a 12 byte long vector with random values)
            var Nonce = new byte[_nonceLen];
            // Creates the tag vector
            var Tag = new byte[_tagLen];

            // Here I use the "using"-keyword because it releases the resources right after using them!
            using var AesGcm = new AesGcm(key, _tagLen);
            // Converts the content into a vector fo bytes
            var ContentBytes = Encoding.UTF8.GetBytes(content);
            // creates a empty byte[] same size of contentBytes
            var CipherContent = new byte[ContentBytes.Length];

            // Now we create the cipher block from content, and the GCM auth tag
            AesGcm.Encrypt(Nonce, ContentBytes, CipherContent, Tag);

            // Creates a matrix with length of tag, nonce, ciphercontent and fill with data
            var EncryptedContent = new byte[Nonce.Length + Tag.Length + CipherContent.Length];
            Buffer.BlockCopy(Nonce, 0, EncryptedContent,0, Nonce.Length);
            Buffer.BlockCopy(Nonce, 0, EncryptedContent,0, Nonce.Length + Tag.Length);
            Buffer.BlockCopy(CipherContent, 0, EncryptedContent, Nonce.Length + Tag.Length, CipherContent.Length);
            return Convert.ToBase64String(EncryptedContent);
        }
        catch (Exception e)
        {
            // Im being intentionally vague in these exceptions
            throw new Exception("Failed to encrypt", e);
        }            
    }

    public static string Decrypt(string encrypted_content, byte[] key)
    {
        try
        {
            var data = Convert.FromBase64String(encrypted_content);
            // Creates a copy of the first 12 bytes
            var Nonce = new byte[_nonceLen];
            Buffer.BlockCopy(data, 0, Nonce,0, Nonce.Length);

            // Creates a copy of the Tag length (almost 16 bytes)
            var Tag = new byte[_tagLen];
            Buffer.BlockCopy(data, _nonceLen, Tag, 0, _tagLen);

            // Creates a copy the cipher by removing the other bytes
            var CipherLen = data.Length - _nonceLen - _tagLen;
            var CipherData = new byte[CipherLen];
            Buffer.BlockCopy(data, _nonceLen + _tagLen, CipherData,0, CipherLen);

            
            using var aesGcm = new AesGcm(key, _tagLen);
            var DecryptedContentBytes = new byte[CipherLen];
            aesGcm.Decrypt(Nonce, CipherData, Tag, DecryptedContentBytes);
            return Encoding.UTF8.GetString(DecryptedContentBytes);
        }
        
        // If the padding is bad/mismatching 
        catch (AuthenticationTagMismatchException)
        {
            throw new Exception("Authentication failed.");
        }
        catch (Exception)
        {
            throw new Exception("Failed to decrypt.");
        }
    }

}
