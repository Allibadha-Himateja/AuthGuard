using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Data;

namespace AuthAppAPI.Controllers
{
    public static class AuthorizationDemoEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorizationDemoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/AdminOnly", AdminOnly);

            app.MapGet("/AdminOrTeacher", [Authorize(Roles = "Admin,Teacher")] () =>
            {
                return "Admin and Teacher";
            });

            app.MapGet("/LibraryMembersOnly", [Authorize(policy: "HasLibraryID")]()=>
            {
                return "Library members only";
            });

            app.MapGet("/ApplyMaternityLeave", [Authorize(Roles ="Teacher",Policy = "FemalesOnly")] () =>
            {
                return "Female Teachers only";
            });
            return app;

            app.MapGet("/Under10Females",
            [Authorize(Policy = "Under10")]
            [Authorize(Policy = "FemalesOnly")] () =>
            {
                return "Females under 10";
            });
        }

        [Authorize(Roles="Admin")]
        public static string AdminOnly()
        {
            return "Admin Only";
        }
    }
}
