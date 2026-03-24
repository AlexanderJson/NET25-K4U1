using Microsoft.Extensions.Options;
using MyWebApi.Api.Interfaces;
using MyWebApi.App.DTO;
using MyWebApi.App.Options;

namespace MyWebApi.App.Services;



public class SecretService(
    ITokenService tokenService,
    IOptions<SecretOptions> options,
    ISecretRepository repo
    ) : ISecretService
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly ISecretRepository _repo = repo;

    private readonly byte[] _key = Convert.FromBase64String(options.Value.Key);

    public CreatedSecretDto CreateSecret(CreateSecretDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (string.IsNullOrWhiteSpace(dto.Content))
            throw new ArgumentException("Content is required.");

        if (dto.MaxViews is <= 0)
            throw new ArgumentException("MaxViews must be greater than 0 when provided.");

        DateTime expiresAt = dto.ExpiresAt.ToUniversalTime();

        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("ExpiresAt must be in the future.");

        var secretId = Guid.NewGuid();
        var accessToken = _tokenService.GenerateToken();
        var aadDto = new AadDto
        {
            SecretId = secretId,
            ExpiresAt = expiresAt,
            MaxViews = dto.MaxViews ?? 0,
            RequiresPassword = false
        };
        var aad = AesGcmUtils.GenerateAad(aadDto);
        var encryptedContent = AesGcmUtils.Encrypt(dto.Content, _key, aad);
        var secret = new Secret
        {
            Id = secretId,
            HashedAccessToken = _tokenService.HashToken(accessToken),
            EncryptedContent = encryptedContent,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt,
            MaxViews = dto.MaxViews,
            CurrentViews = 0,
            RequiresPassword = false
        };
        _repo.Add(secret);
        return new CreatedSecretDto
        {
            Id = secret.Id,
            AccessToken = accessToken,
            ExpiresAt = secret.ExpiresAt,
            MaxViews = secret.MaxViews
        };
    }

    public SecretDto GetByToken(string accessToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        var accessTokenHash = _tokenService.HashToken(accessToken);
        Secret? secret = _repo.GetByToken(accessTokenHash) ?? throw new KeyNotFoundException("Secret not found."); ;
        //TODO gör riktig delete sen
        if (secret.ExpiresAt <= DateTime.UtcNow)
            throw new InvalidOperationException("Secret has expired.");
        //TODO flytta check till entity
        if (secret.MaxViews.HasValue && secret.CurrentViews >= secret.MaxViews.Value)
            throw new InvalidOperationException("Secret is no longer available.");

        var aad = AesGcmUtils.GenerateAad(new AadDto
        {
            SecretId = secret.Id,
            ExpiresAt = secret.ExpiresAt,
            MaxViews = secret.MaxViews ?? 0,
            RequiresPassword = secret.RequiresPassword
        });

        var decryptedContent = AesGcmUtils.Decrypt(secret.EncryptedContent, _key, aad);
        secret.CurrentViews += 1;
        _repo.Update(secret);
        return new SecretDto
        {
            SecretId = secret.Id,
            Content = decryptedContent,
            ExpiresAt = secret.ExpiresAt,
            MaxViews = secret.MaxViews,
            ViewCount = secret.CurrentViews
        };
    }
}


