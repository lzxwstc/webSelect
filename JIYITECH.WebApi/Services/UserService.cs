using JIYITECH.WebApi.Configs;
using JIYITECH.WebApi.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using IdentityModel;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace JIYITECH.WebApi.Services
{

    public interface IUserService
    {
        object Login(string userName, string password);
        bool ChangeUserInfo(JObject user);
        object GetListPaged(int pageNumber, int rowsPerPage, string strWhere, string orderBy, object parameters = null);
        object GetUserInfoByToken(string token);
        bool ChangePassword(long userId, string oldPassword, string newPassword);
        bool ChangeStatus(JObject data);
        User GetUser(long userId);
        bool CreateUser(JObject user);
        bool RemoveUser(long iserId);
        List<Permission> GetPermissionsByUserId(long userId);
        string GenerateTokenWithRole(string role, int expireMinutes = 60);

    }
    public class UserService : IUserService
    {
        private readonly string tokenSecret;
        private readonly AppConfig appConfig;
        private readonly IUnitOfWork uow;
        public UserService(IOptions<AppConfig> appConfig, IUnitOfWork uow)
        {
            this.appConfig = appConfig.Value;
            tokenSecret = this.appConfig.tokenSecret;
            this.uow = uow;
        }

        public bool ChangePassword(long userId, string oldPassword, string newPassword)
        {
            return uow.UserRepository.ChangePassword(userId, oldPassword, newPassword) > 0;
        }

        public bool ChangeStatus(JObject data)
        {
            var userId = data["id"].Value<long>();
            var status = data["status"].Value<bool>();
            return uow.UserRepository.ChangeStatus(userId, status) > 0;
        }

        public bool ChangeUserInfo(JObject user)
        {
            var userId = user["id"].Value<int>();
            var name = user["name"].Value<string>();
            var roles = user["roles"].Value<JArray>();
            return uow.UserRepository.ChangeUserInfo(userId, name, roles);
        }

        public bool CreateUser(JObject user)
        {
            User userToAdd = user.ToObject<User>();

            var salt = GenerateSalt();
            userToAdd.salt = string.Join(",", salt);
            userToAdd.password = Hash(userToAdd.password, salt);
            try
            {
                var res = uow.UserRepository.Add(userToAdd);
                uow.Commit();
                return true;
            }
            catch (Exception)
            {
                uow.Rollback();
                return false;
                //throw;
            }
        }

        private object GenerateToken(string _tokenSecret, User user, List<string> roles, int expireMinutes = 60)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSecret);
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddMinutes(expireMinutes);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Audience,"api"),
                    new Claim(JwtClaimTypes.Issuer,"http://localhost"),
                    new Claim(JwtClaimTypes.Id, user.id.ToString()),
                    new Claim(JwtClaimTypes.Name, user.userName),
                    new Claim(JwtClaimTypes.Role,JsonConvert.SerializeObject( roles)),
                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return new
            {
                token = tokenString,
                tokenType = "Bearer",
                profile = new
                {
                    roles,
                    userId = user.id,
                    user.userName,
                    authTime = authTime.ToLocalTime(),
                    expiresAt = expiresAt.ToLocalTime()
                }
            };
        }

        public string GenerateTokenWithRole(string role, int expireMinutes = 60)
        {
            var tokenSecret = appConfig.tokenSecret;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenSecret);
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddMinutes(expireMinutes);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Audience,"api"),
                    new Claim(JwtClaimTypes.Issuer,"http://localhost"),
                    new Claim(JwtClaimTypes.Role,role),
                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public object GetListPaged(int pageNumber, int rowsPerPage, string strWhere, string orderBy, object parameters = null)
        {
            return uow.UserRepository.GetListPaged(pageNumber, rowsPerPage, strWhere, orderBy, parameters);
        }

        //private ClaimsPrincipal GetPrincipal(string token)
        //{
        //    try
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        if (!(tokenHandler.ReadToken(token) is JwtSecurityToken jwtToken))
        //        {
        //            return null;
        //        }
        //        var symmetricKey = Convert.FromBase64String(tokenSecret);
        //        var validationParameters = new TokenValidationParameters()
        //        {
        //            //去除时间钟摆
        //            ClockSkew = TimeSpan.Parse("00:00:00"),
        //            RequireExpirationTime = true,
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
        //        };
        //        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
        //        return principal;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return new ClaimsPrincipal();
        //    }
        //}

        public User GetUser(long userId)
        {
            return uow.UserRepository.GetModel(userId);
        }

        public object GetUserInfoByToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (!(tokenHandler.ReadToken(token) is JwtSecurityToken jwtToken))
            {
                return null;
            }
            User user = uow.UserRepository.GetModel(long.Parse(jwtToken.Payload["id"].ToString()));
            return new
            {
                user.userName,
                roles = JArray.Parse(jwtToken.Payload["role"].ToString()),
                user.name,
            };
        }
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public object Login(string userName, string password)
        {
            try
            {
                var a = uow.UserRepository.Head();
                uow.UserRepository.Delete(39);
                //uow.UserRepository.Add(new User());
                uow.UserRepository.Delete(31);
                uow.Commit();
            }
            catch (Exception ex)
            {
                uow.Rollback();
            }
            byte[] salt = uow.UserRepository.GetSaltByUserName(userName);
            password = Hash(password, salt);
            User loginUser = uow.UserRepository.GetValidUser(userName, password);
            var saltStr = string.Join(",", salt);
            if (loginUser == null)
            {
                return null;
            }
            IEnumerable<Role> roles = uow.RoleRepository.GetRolesByUserId(loginUser.id);
            List<string> roleNames = new List<string>();
            roles.ToList().ForEach(x => roleNames.Add(uow.RoleRepository.GetModel(x.id).roleName));
            return GenerateToken(appConfig.tokenSecret, loginUser, roleNames, 60);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool RemoveUser(long userId)
        {
            if (uow.UserRepository.Delete(userId) > 0) {
                uow.Commit();
                return true;
            }
            else
            {
                uow.Rollback();
                return false;
            }
        }
        /// <summary>
        /// 哈希加盐
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        private string Hash(string password, byte[] salt)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
        /// <summary>
        ///  generate a 128-bit salt using a secure PRNG
        /// </summary>
        /// <returns></returns>
        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
                return salt;
            }
        }
        /// <summary>
        /// 获取用户全部权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Permission> GetPermissionsByUserId(long userId)
        {
            return uow.UserRepository.GetPermissionsByUserId(userId);
        }
    }
}
