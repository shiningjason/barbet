using Microsoft.AspNetCore.Authentication;

namespace BarBet.Application.Web.Services.Auth;

public static class BarBetAuthenticationExtension
{
    public static AuthenticationBuilder AddBarBet(
        this AuthenticationBuilder builder,
        Action<BarBetAuthenticationSchemeOptions> configureOptions
    )
    {
        return builder.AddScheme<BarBetAuthenticationSchemeOptions, BarBetAuthenticationHandler>
            (BarBetAuthenticationHandler.AuthenticationScheme, configureOptions);
    }
}