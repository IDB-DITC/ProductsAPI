using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductsAPI.Models
{
    //public class BearerTokenHandler : TokenHandler
    //{
    //    private readonly JsonWebTokenHandler _tokenHandler = new();

    //    public override Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
    //    {
    //        try
    //        {
    //            var validatedToken = _tokenHandler.ValidateTokenAsync(token, validationParameters).Result;
    //            validatedToken.
    //            if (validatedToken is not JsonWebToken jwtSecurityToken)
    //                return Task.FromResult(new TokenValidationResult() { IsValid = false });

    //            return Task.FromResult(new TokenValidationResult
    //            {
    //                IsValid = true,
    //                ClaimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims, JwtBearerDefaults.AuthenticationScheme),

    //                // If you do not add SecurityToken to the result, then our validator will fire, return a positive result, 
    //                // but the authentication, in general, will fail.
    //                SecurityToken = jwtSecurityToken,
    //            });
    //        }

    //        catch (Exception e)
    //        {
    //            return Task.FromResult(new TokenValidationResult
    //            {
    //                IsValid = false,
    //                Exception = e,
    //            });
    //        }
    //    }
    //}




}
