using FlowerSpotCore.InterfacesRepository;
using FlowerSpotCore.InterfacesServices;
using FlowerSpotCore.ModelsRepository;
using FlowerSpotCore.ModelsServices;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FlowerSpotLogic
{
    public class UsersService : IUsersService
    {
        private IAllRepositories _allRepositories;

        private string _jwtSecretKey;
        private int _jwtValidSeconds;
        private string _jwtIssuer;
        private string _jwtAudience;

        public UsersService(IAllRepositories AllRepositories)
        {
            _allRepositories = AllRepositories;

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            var config = configuration.Build();

            //if parameters cannot be load from configuration file (unit test), the default values are defined
            _jwtSecretKey = config.GetSection("JwtToken:SecretKey").Value ?? "";
            try
            {
                _jwtValidSeconds = int.Parse(config.GetSection("JwtToken:ValidSeconds").Value ?? "");
            }
            catch
            {
                _jwtValidSeconds = 0;
            }
            _jwtIssuer = config.GetSection("JwtToken:Issuer").Value ?? "";
            _jwtAudience = config.GetSection("JwtToken:Audience").Value ?? "";

        }

        public bool AddUser(string UserName, string Password, string Email)
        {
            if (IsUserNameAndPasswordProvided(UserName, Password) && IsEmailAddressFormatValid(Email))
            {
                if (!IsUserExist(UserName, Email))
                {
                    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes

                    string hashed = GetPasswordHash(Password, salt);

                    _allRepositories.Users.Create(new UserModel(UserName, hashed, Convert.ToBase64String(salt), Email));

                    int result = _allRepositories.Save();

                    return result == 1;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string LoginUser(string UserName, string Password)
        {
            if (IsUserNameAndPasswordProvided(UserName, Password))
            {
                UserModel? user = GetUser(UserName);

                if (user != null)
                {
                    string hashed = GetPasswordHash(Password, Convert.FromBase64String(user.PasswordSalt));

                    if (hashed == user.Password)
                    {
                        return generateToken(_jwtSecretKey, _jwtValidSeconds, _jwtIssuer, _jwtAudience, user.UserId);
                    }
                }
            }
            return "";
        }

        public JwtTokenModel? ValidateTokenAndGetData(string Token)
        {
            if (validateToken(Token, _jwtSecretKey, _jwtIssuer, _jwtAudience))
            {
                return getJwtTokenData(Token);
            }
            else
            {
                return null;
            }
        }

        private static string GetPasswordHash(string Password, byte[] Salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: Password,
                    salt: Salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
        }

        private bool IsUserNameAndPasswordProvided(string UserName, string Password)
        {
            return UserName.Length > 0 && Password.Length > 0;
        }

        private bool IsEmailAddressFormatValid(string EmailAddress)
        {
            if (EmailAddress.Length > 0)
            {
                int indexAt = EmailAddress.IndexOf("@");
                if (indexAt > 0)
                {
                    int indexDot = EmailAddress.IndexOf(".", indexAt);
                    if (indexDot > indexAt)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsUserExist(string UserName, string Email)
        {
            return _allRepositories.Users.FindByCondition(x => x.UserName.Equals(UserName) && x.Email.Equals(Email)).Any();
        }

        private UserModel? GetUser(string UserName)
        {
            List<UserModel> users = _allRepositories.Users.FindByCondition(x => x.UserName.Equals(UserName)).ToList();
            return users.Count == 1 ? users[0] : null;
        }

        private string generateToken(string secretKey, int validSeconds, string issuer, string audience, int UserID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            Dictionary<string, object> claims = new Dictionary<string, object>
            {
                { "userId", UserID }
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = claims,
                Expires = DateTime.UtcNow.AddSeconds(validSeconds), 
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool validateToken(string token, string secretKey, string issuer, string audience)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = key
            };

            try
            {
                // Try to validate the token
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                // If validation is successful, the token is valid
                return true;
            }
            catch (Exception)
            {
                // Token validation failed
                return false;
            }
        }

        private JwtTokenModel? getJwtTokenData(string token)
        {
            JwtTokenModel? jsonTokenData = null;

            int index1 = token.IndexOf(".") + 1;
            int index2 = token.IndexOf(".", index1);
            string tokenPayloadEncoded = token.Substring(index1, index2 - index1);

            int padding = tokenPayloadEncoded.Length % 4;
            if (padding > 0)
            {
                tokenPayloadEncoded += new string('=', 4 - padding);
            }

            string tokenPayload = Encoding.UTF8.GetString(Convert.FromBase64String(tokenPayloadEncoded));

            try
            {
                jsonTokenData = JsonSerializer.Deserialize<JwtTokenModel>(tokenPayload);
            }
            catch { }

            return jsonTokenData;

        }
    }
}
