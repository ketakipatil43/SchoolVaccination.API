using System.IdentityModel.Tokens.Jwt;

namespace DataHelper.Entities
{
    public static class TokenConfiguration
    {

        public static TokenClaimModel ApplyToken(JwtSecurityToken jwtToken, string strToken)
        {
            var _tokenClaim = new TokenClaimModel
            {
                Token = strToken,
                RoleId = GetClaimValueAsInt(jwtToken, "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid"),
                UniqueId = GetClaimValueAsInt(jwtToken, "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid"),
                Role = GetClaimValueAsString(jwtToken, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"),
                Name = GetClaimValueAsString(jwtToken, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"),
            };

            return _tokenClaim;
        }

        private static int GetClaimValueAsInt(JwtSecurityToken jwtToken, string claimType)
        {
            try
            {
                var value = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
                return int.TryParse(value, out var result) ? result : 0;
            }
            catch { return 0; }


        }

        private static bool GetClaimValueAsBool(JwtSecurityToken jwtToken, string claimType)
        {
            try
            {
                if (bool.TryParse(jwtToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value, out var result))
                {
                    return result; // Return the actual parsed boolean value (True/False)
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static string GetClaimValueAsString(JwtSecurityToken jwtToken, string claimType)
        {
            try
            {
                return jwtToken.Claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? string.Empty;
            }
            catch { return string.Empty; }
        }
    }
}
