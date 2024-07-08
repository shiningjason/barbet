using BarBet.Application.Web.Models.Config;
using BarBet.Common.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarBet.Application.Web.Controllers;

[ApiController]
[Route("api/configs")]
public class ConfigController
{
    private static readonly Lazy<ConfigModel> _config = new(() => new ConfigModel
    {
        GoogleAuthClientId = Config.Web.GoogleAuthentication.ClientId
    });

    [HttpGet]
    [AllowAnonymous]
    public ResponseBaseModel<ConfigModel> GetConfigs()
    {
        return new ResponseBaseModel<ConfigModel>(COMM0000, _config.Value);
    }
}