using System;
using Microsoft.AspNetCore.Http;

namespace PA.Helpers
{
    public static class ContextHelper
    {
        /// <summary>
        /// Gets the currently logged in user's credentials
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <returns>a User object</returns>
        public static User GetCurrentUser(this HttpContext http)
        {
            User user = new User();
            foreach (var claim in http.User.Claims)
            {
                if (claim.Type == "Id")
                {
                    user.Id = Int32.Parse(claim.Value);
                }
                else if (claim.Type == "Username")
                {
                    user.Username = claim.Value;
                }
                else
                {
                    user.Role = claim.Value;
                }
            }
            return user;
        }
    }
}