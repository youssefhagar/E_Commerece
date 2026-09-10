using E_Commerece.Application.Contracts;
using E_Commerece.Application.Dtos.AuthDtos;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerece.Infrastructure.Services
{
    public class JWTAccessTokenGenerator(IOptions<JwtSetting> options)
        : IAcessTokenGenerator
    {

        private readonly JwtSecurityTokenHandler _handler = new();
        private readonly JwtSetting _jwtSetting = options.Value;
        public string GenerateToken(UserDTo userInfo, IEnumerable<string> Roles)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(userInfo,nameof(userInfo));
            if(userInfo.DisplayName == null) throw new ArgumentNullException(nameof(userInfo), "Name Required");
            if(userInfo.UserName == null) throw new ArgumentNullException(nameof(userInfo),"Username Required");
            if(userInfo.Eamil == null) throw new ArgumentNullException(nameof(userInfo),"Email Requird");

            //Claims
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userInfo.Id),
                //new(ClaimTypes.Upn, userInfo.UserName),
                new(ClaimTypes.Email, userInfo.Eamil),
                new(ClaimTypes.Name, userInfo.DisplayName),
            };
            claims.AddRange(Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            //Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Secert));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSetting.Issuer,
                audience: _jwtSetting.Audience,
                expires:DateTime.UtcNow.AddMinutes(_jwtSetting.ExpireMinutes),
                claims: claims,
                signingCredentials: credentials
                );

            //return
            return _handler.WriteToken(token);
        }
    }
}
