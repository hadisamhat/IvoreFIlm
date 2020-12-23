using System;
using System.Linq;
using System.Security.Authentication;
using IvoreFilm.Helpers.TokenHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IvoreFilm.Helpers.Authorization
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuth : Attribute, IAuthorizationFilter
    {
        private readonly DecodeJwtToken _tokenHelper = new DecodeJwtToken();
        private string _role;

        public CustomAuth(string roles)
        {
            _role = roles;

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")[1];
                var userRole = _tokenHelper.GetRole(token);
                if (userRole != _role && userRole != "Admin")
                {
                    context.Result = new JsonResult(new {message = "Unauthorized"})
                        {StatusCode = StatusCodes.Status401Unauthorized};
                }
                 
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        


    }
}