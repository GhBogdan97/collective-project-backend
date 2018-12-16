using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UserIdClaimExtension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            var claim =  user.Claims.FirstOrDefault(u => u.Type.Contains("nameidentifier"));
            if(claim == null)
            {
                return string.Empty;
            }
            return claim.Value;
        }
    }
}
