namespace AuthAppAPI.Extentions
{
    public static class AppConfigExtentions
    {
        public static WebApplication ConfigureCORS(this WebApplication app,IConfiguration config)
        {
            // order of using the middlewares like CORS,UseAuthentication,UseAuthorization
            // this is the way of using the middlewares first thing is cors>authentication>authorization
            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:8081", "http://localhost:4200", "http://localhost:8082")
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
            return app;
        }
    }
}
