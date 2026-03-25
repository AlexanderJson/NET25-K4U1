public interface ISecretRepository
{
    Task Add(Secret secret);
    Task<Secret?> GetByToken(byte[] hashedAccessToken);
    Task<List<UserSecretList>> GetUserSecrets(Guid userId);

    Task Update(Secret secret);
}
