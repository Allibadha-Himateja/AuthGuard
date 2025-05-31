using AuthAppAPI.Extentions;
using AuthAppAPI.Models;
using AuthAppAPI.Controllers;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// THIS IS HOW WE HAVE ORGANISED OUR LOGIC WITH THE HELP OF THE EXTENTION CLASSES 
// FOR THAT I HAVE USED THE AuthAppApi/Extentions Folder
// look the logic inside the Extentions Folder 
// this Extentions are in order
// 1) setting DbContext
// 2) setting the IdentityUser and the DBContext to Store
// 3) configaring the validation models like IdentityUser model using the IdentityOptions
// 4) Configuring the JWT Authentication as well as the Authorization AddIdentityAuth
builder.Services.AddSwaggerExplorer()
                .InjectDbContext(builder.Configuration)
                .AddIdentityhandlersStores()
                .ConfigureIdentityOptions()
                .AddIdentityAuth(builder.Configuration);

// providing a class for injecting the connection string (AppSettings Class with property JWTSecret)
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


var app = builder.Build();

app.ConfigureSwaggerExplorer()
    .ConfigureCORS(builder.Configuration)
    .AddIdentityAuthMiddlewares(); // configuration is just for safety


app.MapControllers();
// this MapIdentityApi will provide you with all of the api endpoints like \register,\login...etc
app.MapGroup("/api")
    .MapIdentityApi<AppUser>();
app.MapGroup("/api")
    .MapIdentityUserEndpoints()
    .MapAccountEndpoints()
    .MapAuthorizationDemoEndpoints();
    //////// EE MapIdentityUserEndpoints() method is in the controller 
    // SINCE IT IS A WEBAPI ACTION/ A PROPER ENDPPOINT SO WE HAVE KEPT THAT INSIDE THE CONTROLLERS FOLDER

// this api endpoint require the builder.configuration and sending them direclty is not a correct practise
// for that we have to create the property class called AppSettings which will load the ConString to all the other extentions classes
// THAT TOO USING THE DEPENDENCY INJECTION


app.Run();