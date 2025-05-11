using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace DataHelper.Entities
{
    public enum AuthorizeLevel
    {
        None = 0,
        AccessToken = 1,
    }
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class LTAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public const string TokenExpire = "Your Token is expired! Please relogin.";
        public const string Somethingwrong = "Something went wrong, please contact application support team";
        public const string InvalidToken = "Invalid Token / Token not found / Unable to Parse Token";
        public const string InvalidSecretKey = "Invalid Secret Key / Secret Key not found";

        public List<AuthorizeLevel> SecurityLevels { get; } = new List<AuthorizeLevel>();

        public LTAuthorizeAttribute(params AuthorizeLevel[] securityLevels)
        {
            if (securityLevels.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("Invalid Security Levels in LTAuthorizeAttribute");
            securityLevels.ToList().ForEach(y => SecurityLevels.Add(y));
        }

        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            AuthorizeLevel levelToCheck = AuthorizeLevelCheck();

            switch (levelToCheck)
            {
                case AuthorizeLevel.None:
                    return;
                case AuthorizeLevel.AccessToken:
                    CheckAccessToken(context);
                    break;
            }
        }

        private void CheckAccessToken(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
            if (!ParseAccessToken(context, token ?? string.Empty))
            {
                ReturnFailedResult(context);
            }

        }
        private void CheckAzureToken(AuthorizationFilterContext context)
        {
            var azureToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(azureToken);
            if (jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name") != null)
            {
                var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name").Value ?? string.Empty;
                if (string.IsNullOrEmpty(email))
                {
                    ReturnFailedResult(context);
                }
            }
            else if (!ParseAccessToken(context, azureToken ?? string.Empty))
            {
                ReturnFailedResult(context);
            }

        }

        private AuthorizeLevel AuthorizeLevelCheck()
        {
            if (SecurityLevels.Contains(AuthorizeLevel.None))
            {
                return AuthorizeLevel.None;
            }
            else if (SecurityLevels.Contains(AuthorizeLevel.AccessToken))
            {
                return AuthorizeLevel.AccessToken;
            }
            return AuthorizeLevel.None;
        }
        private bool ParseAccessToken(AuthorizationFilterContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(AppSettings.AppConfig.JwtSettings.Key);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                if (jwtToken != null)
                {
                    TokenClaimModel tokenClaimModel = TokenConfiguration.ApplyToken(jwtToken, token);
                    context.HttpContext.Items.Remove("tokenClaimModel");
                    context.HttpContext.Items.Add("tokenClaimModel", tokenClaimModel);

                    return true;
                }
            }
            catch (Exception ex)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    error = "An error occurred during authorization. " + ex.Message
                });
            }
            return false;
        }

        protected void ReturnFailedResult(AuthorizationFilterContext context)
        {
            try
            {
                context.Result = new UnauthorizedObjectResult("An error occurred during authorization. ");
            }
            catch (Exception ex)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    error = "An error occurred during authorization. " + ex.Message
                });
            }

        }

        private static string GetClientIPAddress(HttpContext httpContext)
        {
            string forwardedFor = Convert.ToString(httpContext.Request.Headers["X-Forwarded-For"]);
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                return forwardedFor;
            }

            return Convert.ToString(httpContext.Connection.RemoteIpAddress) ?? "";
        }
    }
}
