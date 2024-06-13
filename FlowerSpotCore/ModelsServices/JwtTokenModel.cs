namespace FlowerSpotCore.ModelsServices
{
    public class JwtTokenModel
    {
        public int? userId { get; set; } = null;
        public long? nbf { get; set; } = null;
        public long? exp { get; set; } = null;
        public long? iat { get; set; } = null;
        public string? iss { get; set; } = null;
        public string? aud { get; set; } = null;

        public DateTime NotBeforeTimeUtc
        {
            get
            {
                if (nbf != null)
                {
                    return new DateTime(1970, 1, 1).AddSeconds(nbf.Value);
                }
                else
                {
                    return DateTime.MaxValue;
                }
            }
        }

        public DateTime ExpireTimeUtc
        {
            get
            {
                if (exp != null)
                {
                    return new DateTime(1970, 1, 1).AddSeconds(exp.Value);
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }
    }
}
