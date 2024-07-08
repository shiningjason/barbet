using BarBet.Application.Web.Models.Auth;
using BarBet.Common.Web.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarBet.Application.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController
{
    private readonly Lazy<GoogleJsonWebSignature.ValidationSettings> _googleValidationSetting = new(() =>
        new GoogleJsonWebSignature.ValidationSettings
            { Audience = new[] { Config.Web.GoogleAuthentication.ClientId } });

    [HttpPost("google")]
    [AllowAnonymous]
    public async Task<ResponseBaseModel<string>> GoogleAuthenticate([FromBody] GoogleAuthenticateRequestModel request)
    {
        await GoogleJsonWebSignature.ValidateAsync(request.Credential, _googleValidationSetting.Value);
        return new ResponseBaseModel<string>(COMM0000, string.Empty);
    }
}