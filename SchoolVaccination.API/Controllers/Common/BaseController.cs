using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataHelper.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UM.Core.ModelEntities;

namespace SchoolVaccination.API.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    [LTAuthorize(AuthorizeLevel.AccessToken)]
    public class BaseController : Controller
    {
        public string CallParams { get; set; }
        public BaseController()
        {
        }

        protected string GetAccessToken(CreateAccountModel students)
        {
            Claim[] claims = new Claim[]
            {
                new(ClaimTypes.PrimaryGroupSid,  students.UniqueId.ToString()),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (ClaimTypes.Role, "Admin"),
                new (ClaimTypes.PrimarySid, students.UniqueId.ToString()),
                new (ClaimTypes.Name, students.FirstName+" "+students.LastName)
            };
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(AppSettings.AppConfig.JwtSettings.Key));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken securityToken = new(
                issuer: AppSettings.AppConfig.JwtSettings.Issuer,
                audience: AppSettings.AppConfig.JwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(AppSettings.AppConfig.JwtSettings.MinutesToExpiration),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CallParams = JsonConvert.SerializeObject(context.ActionArguments);
            if (!context.ActionArguments.TryGetValue("formData", out object formData) && !context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary());
            }
            base.OnActionExecuting(context);
        }
    }
}
