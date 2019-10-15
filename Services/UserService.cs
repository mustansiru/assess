using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using assess.Helpers;
using assess.Models;
using assess.Services.Intefaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace assess.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, IDatabaseSettings dbSettings)
        {
            _appSettings = appSettings.Value;

            var client = new MongoClient(dbSettings.ConnectionString);
            var database = client.GetDatabase(dbSettings.DatabaseName);
            _users = database.GetCollection<User>("Users");
        }

        public User Authenticate(string username, string password)
        {
            var encryptedPassword = GenerateHash(password);
            var user = _users.Find(x => x.Username == username && x.Password == encryptedPassword).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            // authentication succesful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        public IEnumerable<User> GetAll()
        {
            return _users.Find(x => true).ToList().WithoutPasswords();
        }

        private string GenerateHash(string plainText)
        {
            var keyHash = "EebChARO9lWx88HLzlQQPaaoKzxfYDGiG0B09IGW";
            var blockSize = 128;

            using (Aes aesAlg = Aes.Create())
            {
                byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
                var hash = MD5.Create();
                var key = hash.ComputeHash(Encoding.Unicode.GetBytes(keyHash));

                aesAlg.BlockSize = blockSize;
                byte[] encrypted = plainText.EncryptString(key, IV);

                return Convert.ToBase64String(encrypted);
            }
        }
    }
}
