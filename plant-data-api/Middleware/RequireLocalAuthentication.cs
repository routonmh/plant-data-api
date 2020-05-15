using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace PlantDataAPI.Middlewares
{
    /// <summary>
    ///
    /// </summary>
    public class RequireLocalAuthentication
    {
        private readonly RequestDelegate next;

        private TokenValidationParameters validationParameters;
        private JwtSecurityTokenHandler tokenHandler;

        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        public RequireLocalAuthentication(RequestDelegate next)
        {
            this.next = next;

            string jwtSecret = Program.JwtSigningKey;
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(jwtSecret));
            validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = signingKey,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, // We check session lifetime via DB (enables refresh control)
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            bool isAuthorized = false;
            string jwt = context.Request.Headers["Authorization"];

            SecurityToken securityTokenResult = null;

            if (tokenHandler.CanReadToken(jwt))
                try
                {
                    // Validate token, exception thrown if does not comply with validationParameters
                    tokenHandler.ValidateToken(jwt, validationParameters, out securityTokenResult);
                    JwtSecurityToken token = tokenHandler.ReadJwtToken(jwt);
                    isAuthorized = true;
                }
                catch (Exception ex)
                {
                    // Failed to authenticate
                }


            // Set Header with
            if (isAuthorized)
                await next(context);
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("401 - Unauthorized.");
            }
        }
    }
}