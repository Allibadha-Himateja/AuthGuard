using AuthAppAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthAppAPI.Controllers
{
    public static class AccountEndpoints
    {
        public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/UserProfile", GetUserProfile);
            return app;
        }

        [Authorize]
        public static async Task<IResult> GetUserProfile(ClaimsPrincipal user,UserManager<AppUser> userManager)
        {
            string userID=user.Claims.First( x => x.Type == "UserID").Value;
            var UserDetails=await userManager.FindByIdAsync(userID);
            return Results.Ok(
                new
                {
                    Email = UserDetails?.Email,
                    FullName = UserDetails?.Fullname,
                });
        }
    }
}
