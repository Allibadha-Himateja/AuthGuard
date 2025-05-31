using AuthAppAPI.Controllers;
using AuthAppAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthAppAPI.Extentions
{
    public class UserRegistrationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int? LibraryID { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public static class IdentityUserEndpoints
    {
        // this extention handle the api endpoints 
        // in other word to say that is extention handles all the controller webapi actions
        // IN OTHER TO HANDLE THE API ENDPOINT WE HAVE TO USE THE IEndpointRouteBuilder  INSTEAD OF THE IServiceCollection
        public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app)
        {
            // this allow anonymous we can able to access them even before the authentication
            app.MapPost("/signup", CreateUser).AllowAnonymous();
            app.MapPost("/signin", SignIn).AllowAnonymous();
            return app;
        }

        public async static Task<IResult> CreateUser(UserManager<AppUser> usermanager, [FromBody] UserRegistrationModel userRegistrationModel)
        {
            AppUser user = new AppUser()
            {
                // in the end point i am trying to validate or ignore all the warnings which are triggered when running the app
                // the Identity is asking for the UserName Attribute 
                // we have to handle it from here like username as email itseelf
                UserName = userRegistrationModel.Email,
                Email = userRegistrationModel.Email,
                Fullname = userRegistrationModel.FullName,
                Gender= userRegistrationModel.Gender,
                DOB=DateOnly.FromDateTime(DateTime.Now.AddYears(-userRegistrationModel.Age)),
                LibraryID = userRegistrationModel.LibraryID,
            };
            var result = await usermanager.CreateAsync(user, userRegistrationModel.Password);
            // createAsync method will do all the sql db logic 
            //builder.Services.AddIdentityApiEndpoints<AppUser>()
            //.AddEntityFrameworkStores<AppDbContext>();

            // we have to add the role from the identity AppUser property into the ASPNetRoles table for organizing the Roles
            // we have to manually add the role data from the userRegistration model into the db by using the usermanager 
            await usermanager.AddToRoleAsync(user, userRegistrationModel.Role);


            if (result.Succeeded)
            {
                return Results.Ok(result);
            }
            else
            {
                return Results.BadRequest(result);
            }
        }

        public async static Task<IResult> SignIn(UserManager<AppUser> usermanager, [FromBody] LoginModel loginmodel,IOptions<AppSettings> appSettings)
        {
            var user = await usermanager.FindByEmailAsync(loginmodel.Email);
                if (user != null && await usermanager.CheckPasswordAsync(user, loginmodel.Password))
                {
                    var roles = await usermanager.GetRolesAsync(user); 
                    var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.JWTSecret));

                    ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                    {
                            new Claim("UserID", user.Id.ToString()),
                            new Claim("Gender",user.Gender.ToString()),
                            new Claim("Age",(DateTime.Now.Year - user.DOB.Year).ToString()),
                            new Claim(ClaimTypes.Role,roles.First())
                    });

                    if (user.LibraryID != null) {
                        claims.AddClaim(new Claim("LibraryID", user.LibraryID.ToString()!));
                    }
                    var tokendescriptor = new SecurityTokenDescriptor
                    {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddDays(10),
                        SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256)
                    };
                    var tokenhandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenhandler.CreateToken(tokendescriptor);
                    var token = tokenhandler.WriteToken(securityToken);
                    return Results.Ok(new { token });
                }
                else
                {
                    return Results.BadRequest(new { message = "UserName or the password is incorrect" });
                }
        }
    }
}

