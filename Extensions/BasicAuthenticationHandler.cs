using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace EmployeeApi.Extensions;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ILogger<BasicAuthenticationHandler> _logger;
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ILogger<BasicAuthenticationHandler> nlogger) : base(options, logger, encoder, clock)
    {
        _logger = nlogger;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        //auth: Basic dXNlcm5hbWU6cGFzc3dvcmQ=
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Authorization Header valid");
            //TOdo - check user credentials            
            var token = authHeader.Substring("Basic ".Length).Trim(); //dXNlcm5hbWU6cGFzc3dvcmQ=
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(token)); //username:password
            var credentials = decodedCredentials.Split(":"); //credntials[0]: username, credntials[1]: password
            //call method to check provided username/password
            //if(_context.authenticate(username,password))
            if (credentials[0] == "Admin" && credentials[1] == "Admin")
            {
                _logger.LogInformation($"Authentication success: User - {credentials[0]}");
                var claims = new[] {
                    new Claim("name", credentials[0]),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                var identity = new ClaimsIdentity(claims, "Basic");
                var claimsPrincipal = new ClaimsPrincipal(identity);
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
            }
            _logger.LogInformation($"Authentication failed: User - {credentials[0]}");
        }

            _logger.LogError("Invalid Authorization Header");
            Response.StatusCode = 401;
            Response.Headers.Add("WWW-Authenticate", "Basic realm=\"EmployeeApi\"");
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
    }
}