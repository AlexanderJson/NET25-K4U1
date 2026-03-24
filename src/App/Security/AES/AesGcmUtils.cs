

using System.Security.Cryptography;
using System.Text;

/// <summary>
/// The main encryption/decryption logic
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
    public static string Encrypt(string content, byte[] key, byte[] aad)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(content);
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(aad);
            if(key.Length != 32) throw new ArgumentException("Incorrect key size!");

            // Creates the nonce (a 12 byte long vector with random values)
            var Nonce = new byte[_nonceLen];
            // Generates higher entropy random numbers
            RandomNumberGenerator.Fill(Nonce);

            // Creates the tag vector
            var Tag = new byte[_tagLen];

            // Converts our plaintext to bytes
            var ContentBytes = Encoding.UTF8.GetBytes(content);
            // this will be used for the ciphertext
            var CipherContent = new byte[ContentBytes.Length];

            // using released disposes the resources after its done
            using var AesGcm = new AesGcm(key, _tagLen);


            // Now we run the encryption to generate our encrypted content + authentication tag
            AesGcm.Encrypt(Nonce, ContentBytes, CipherContent, Tag, aad);
            /* 
            AesGcm returns three outputs (nonce, ciphercontent, tag).
            So to return the full encrypted data, we first have to 
            make an array the size of these combined, then copy them into 
            that array in the order theyre supposd to be. 
            */
            var EncryptedContent = new byte[Nonce.Length + Tag.Length + CipherContent.Length];
            Buffer.BlockCopy(Nonce, 0, EncryptedContent,0, Nonce.Length);
            Buffer.BlockCopy(Tag, 0, EncryptedContent, Nonce.Length, Tag.Length);            
            Buffer.BlockCopy(CipherContent, 0, EncryptedContent, Nonce.Length + Tag.Length, CipherContent.Length);
            return Convert.ToBase64String(EncryptedContent);
        }
        catch (Exception e)
        {
            // Im being intentionally vague in these exceptions
            throw new Exception("Failed to encrypt", e);
        }            
    }

    public static string Decrypt(string encryptedContent, byte[] key, byte[] aad)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(encryptedContent);
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(aad);


            if (key.Length != 32)
                throw new ArgumentException("Incorrect key size!");

            var data = Convert.FromBase64String(encryptedContent);
            if (data.Length < _nonceLen + _tagLen)
                throw new ArgumentException("Encrypted content is too short.");
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
            var DecryptedContentBytes = new byte[CipherLen];

            
            using var aesGcm = new AesGcm(key, _tagLen);
            aesGcm.Decrypt(Nonce, CipherData, Tag, DecryptedContentBytes,aad);
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

    /// <summary>
    /// We encrypt not only the content, but also some metadata 
    /// about the content. This is a nice way to ensure that things like
    /// expiration date etc is not tampered with. This is only applied to
    /// metadata that is not dynamic. So if the user wants to change any of these
    /// then the content will have to be encrypted again. Which will be implemented later if I have the time.
    /// </summary>
    /// <param name="secretId"></param>
    /// <param name="expiresAt"></param>
    /// <param name="maxViews"></param>
    /// <param name="requiresPassword"></param>
    /// <returns></returns>

    public static byte[] GenerateAad(AadDto aad)
    {
        ArgumentNullException.ThrowIfNull(aad);
        var aadJoined = $"{aad.SecretId:N}|{aad.ExpiresAt:O}|{aad.MaxViews}|{aad.RequiresPassword}";
        return Encoding.UTF8.GetBytes(aadJoined);
    }

}
