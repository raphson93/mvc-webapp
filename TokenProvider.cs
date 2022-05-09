using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using mvc_webapp.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace mvc_webapp
{
    public class TokenProvider
    {
        private readonly DataContext _context;
        
        public string token { get; set; }
        public TokenProvider(DataContext context)
        {
            _context = context;
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
                
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static IEnumerable<Claim> GetUserClaims(Login user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("NAME", user.Username)
            };

            return claims;
        }

        public (string, DateTime?) LoginUser(Login login)
        {

            var JWToken = new JwtSecurityToken();

            using (var sha256hash = SHA256.Create())
            {
                if(!String.IsNullOrEmpty(login.Password))
                {
                    var dbUser = _context.Login.FirstOrDefault(x => x.Password == GetHash(sha256hash, login.Password));

                    //Get user details for the user who is trying to login

                    //Authenticate User, Check if it’s a registered user in Database
                    if (dbUser == null)
                    {
                        return (null, null);
                    }

                    if (dbUser.SessionEnd.HasValue && dbUser.SessionEnd.Value > DateTime.Now)
                    {
                        return (dbUser.SessionKey, dbUser.SessionEnd.Value);
                    }

                    //convert utc to local timezone
                    /*var date = DateTime.Now.ToString();
                    DateTime convertedDate = DateTime.SpecifyKind(
                        DateTime.Parse(date),
                        DateTimeKind.Utc);
                    DateTime dt = convertedDate.ToLocalTime();*/

                    var dt = DateTime.Now;

                    //Authentication successful, Issue Token with user credentials
                    //Provide the security key which was given in the JWToken configuration in Startup.cs
                    var key = Encoding.ASCII.GetBytes
                              ("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
                    //Generate Token for user 
                    JWToken = new JwtSecurityToken(
                        //issuer: "http://localhost:5001/",
                        //audience: "http://localhost:5001/",
                        issuer: "https://rihk-clsv2.ap-southeast-1.elasticbeanstalk.com/",
                        audience: "https://rihk-clsv2.ap-southeast-1.elasticbeanstalk.com/",
                        claims: GetUserClaims(dbUser),
                        notBefore: new DateTimeOffset(dt).DateTime,
                        expires: new DateTimeOffset(dt.AddHours(1)).DateTime,
                        //expires: new DateTimeOffset(dt.AddMinutes(2)).DateTime,
                        //Using HS256 Algorithm to encrypt Token
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                                            SecurityAlgorithms.HmacSha256Signature)
                    );

                    token = new JwtSecurityTokenHandler().WriteToken(JWToken);

                    dbUser.LastLogin = DateTime.Now.ToLocalTime();
                    dbUser.LoginCounter += 1;
                    dbUser.SessionKey = token;
                    dbUser.SessionStart = JWToken.ValidFrom.ToLocalTime();
                    dbUser.SessionEnd = JWToken.ValidTo.ToLocalTime();
                    _context.Update(dbUser);
                    _context.SaveChanges();
                    
                }
                return (token, JWToken.ValidTo.ToLocalTime());
            }
        }

        [HttpPost]
        public string LogoutUser(string JsonLocalStorageObj)
        {
            var dbUser = _context.Login.FirstOrDefault(x => x.Username == JsonLocalStorageObj);
            dbUser.SessionKey = string.Empty;
            dbUser.SessionStart = null;
            dbUser.SessionEnd = null;
            _context.Update(dbUser);
            _context.SaveChanges();
            return token;
        }
    }
}
