using Chamedoon.Domin.Base;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chamedoon.Application.Services.Account.Authentication.Query
{
    public class GenerateJsonWebTokenQuery : IRequest<BaseResult_VM<JwtSecurityToken>>
    {
        public List<Claim>? Claims { get; set; }
    }
    public class GenerateJsonWebTokenQueryHandler : IRequestHandler<GenerateJsonWebTokenQuery, BaseResult_VM<JwtSecurityToken>>
    {
        #region Property
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        public GenerateJsonWebTokenQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Method
        public async Task<BaseResult_VM<JwtSecurityToken>> Handle(GenerateJsonWebTokenQuery request, CancellationToken cancellationToken)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: request.Claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new BaseResult_VM<JwtSecurityToken>
            {
                Result = token,
                Code = 0,
                Message = "",

            };
        }

        #endregion
    }

}
