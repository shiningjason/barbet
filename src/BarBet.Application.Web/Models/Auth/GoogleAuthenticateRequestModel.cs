using System.ComponentModel.DataAnnotations;

namespace BarBet.Application.Web.Models.Auth;

public class GoogleAuthenticateRequestModel
{
    [Required]
    public required string Credential { get; set; }
}