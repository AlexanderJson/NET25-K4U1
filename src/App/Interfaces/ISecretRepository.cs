public interface ISecretRepository
{
    void Add(Secret secret);
    Secret? GetByToken(byte[] HashedAccessToken);
    void Update(Secret secret);
}
