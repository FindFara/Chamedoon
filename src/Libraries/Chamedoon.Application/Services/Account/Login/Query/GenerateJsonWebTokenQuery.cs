using Chamedoon.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chamedoon.Application.Services.Account.Login.Query
{
    public class GenerateJsonWebTokenQuery : IRequest<OperationResult<JwtSecurityToken>>
    {
        public List<Claim>? Claims { get; set; }
    }
    public class GenerateJsonWebTokenQueryHandler : IRequestHandler<GenerateJsonWebTokenQuery, OperationResult<JwtSecurityToken>>
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
        public async Task<OperationResult<JwtSecurityToken>> Handle(GenerateJsonWebTokenQuery request, CancellationToken cancellationToken)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!) );

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(7),
                claims: request.Claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return OperationResult<JwtSecurityToken>.Success(token);
        }
        #endregion
    }
}
