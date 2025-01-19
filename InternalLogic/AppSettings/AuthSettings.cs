using System.Security.Cryptography;

namespace TwitchClips.InternalLogic.AppSettings
{
    public record AuthSettings
    {
        public required string Audience { get; init; }
        public required string IssuerSigningKey { get; init; }
        public required TimeSpan ExpirationTime { get; init; }
        public required int IterationCount { get; init; }
        public required int SaltSize { get; init; }
        public required string HashAlgorithmOID { get; init; }
        public HashAlgorithmName HashAlgorithm { get { return HashAlgorithmName.FromOid(HashAlgorithmOID); } }
    }
}