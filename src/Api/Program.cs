// TODO - ta bort Program.cs från tools sen, kolla upp program.cs och regler där
// TODO + kolla .https filerna

using MyWebApi.Api.Configuration;
using MyWebApi.App.Demo;
using MyWebApi.App.Options;
using MyWebApi.Infrastructure.Data;
using Scalar.AspNetCore;

namespace MyWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsDevelopment())
        {
            var jwtKey = builder.Configuration["Jwt:Key"];
            var secretKey = builder.Configuration["SecretEncryption:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                var generate = DemoKeyGen.GenerateBase64Key(32);
                builder.Configuration["Jwt:Key"] = generate;
                Console.WriteLine($"IN DemO MODE!! Generated JWT Key: {generate}");
            }

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                var generate = DemoKeyGen.GenerateBase64Key(32);
                builder.Configuration["SecretEncryption:Key"] = generate;
                Console.WriteLine($"[DEV] Generated Secret Key: {generate}");
            }
        }
        builder.Services.Configure<SecretOptions>(
        builder.Configuration.GetSection("SecretEncryption"));
        builder.Services
            .AddAppConfig()
            .AddJwTValidation(builder.Configuration)
            .AddRateLimits(builder.Configuration)
            .AddServices()
            .AddExternalApi(builder.Configuration)
            .AddDatabase(builder.Configuration);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseCors("ProdPolicy");
        }

        if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.MapScalarApiReference(options =>
                {
                    options.WithTitle("DEV API ")
                        .WithTheme(ScalarTheme.Moon);
                });
            }

        app.UseHttpsRedirection();
        //app.UseDefaultFiles();
        //app.UseStaticFiles(); 
        app.UseCors("DevPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter();
        app.MapControllers();
        app.Run();
    }
}