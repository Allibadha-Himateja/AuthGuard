using AuthAppAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthAppAPI.Extentions
{
    // this is a Extentions Class we are going to implement it with these classes
    //THE MOST IMPORTANT PART IS THESE CLASSES SHOULD ALL BE STATIC CLASSES
    public static class IdentityEntentions
    {
        // AND FOR DECLARING THE METHODS FOR AN EXTENTION CLASS WE HAVE TO MAKE THEM STATIC
        // AND ALSO WE HAVE TO PASS THE ARGUMENTS WITH A PREFFIX ""THIS""
        // THIS KEYWORD MUST BE USED FOR EVERY METHOD PARAMETERS DECLARATION INSIDE THE ()
        public static IServiceCollection AddIdentityhandlersStores(this IServiceCollection services)
        {
            //Fetching the services form the identity Service Provided by the ASP.NET Core
            //********* using the identity Service specifically the IdentityUser Service
            services.AddIdentityApiEndpoints<AppUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            // changing or manipulating the build in IdentityOptions model's Validation on properties
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = true;
            });

            return services;
        }

        //Auth = Authentication+Authorization
        public static IServiceCollection AddIdentityAuth(this IServiceCollection services,IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(y =>
            {
                y.SaveToken = false;
                y.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AppSettings:JWTSecret"]!)),
                    // ValidateIssuer is for not showing the url of the api which have generated the token from the spa
                    // validateAudience is for not showing not showing the Users of the spa(client side application)
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //this the way of Authorization // basic Authorization
            // like the basic authorization is that we have to be authenticated in order to access the application
            // but for the signin and signup page we have to manually set them to No Authentication
            // for the signin and signup we have to use the[AllowAnonymous]
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                        .RequireAuthenticatedUser()
                                        .Build();
                options.AddPolicy("HasLibraryID", policy => policy.RequireClaim("LibraryID"));
                options.AddPolicy("FemalesOnly", policy => policy.RequireClaim("Gender", "Female"));
                options.AddPolicy("Under10", policy => policy.RequireAssertion(
                    context => Int32.Parse(context.User.Claims.First(x => x.Type == "Age").Value) < 10)
                );
            });


            return services;

        }


        public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
