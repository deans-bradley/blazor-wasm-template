using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BLL.Services;
using BLL.Services.Interfaces;
using Models.Internal;

namespace API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            IConfigurationBuilder configBuilder = EnvironmentBuilder(builder);
            builder.Configuration.AddConfiguration(configBuilder.Build());

            builder.Services.AddFastEndpoints();
            builder.BuildAuthentication();
            builder.BuildAuthorization();

            // Register services
            builder.Services.AddTransient<IJwtService, JwtService>();
            builder.Services.AddTransient<IWeatherService, WeatherService>();

            WebApplication app = builder.Build();

            // Middlewares
            app.RunTimeEnvironment();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.RegisterUI();
            app.UseFastEndpoints();

            app.Run();
        }

        private static void RunTimeEnvironment(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }
        }

        private static void BuildAuthentication(this WebApplicationBuilder builder)
        {
            JwtSettings? jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            if (jwtSettings != null)
            {
                byte[] key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Cookies["AuthToken"];
                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            }
        }

        private static void BuildAuthorization(this WebApplicationBuilder builder)
        {
            // Add your roles here
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("All Users", x => x.RequireRole("Admin", "User"))
                .AddPolicy("Admin", x => x.RequireRole("Admin"));
        }

        private static IConfigurationBuilder EnvironmentBuilder(WebApplicationBuilder builder)
        {
            return new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
        }

        private static void RegisterUI(this WebApplication app)
        {
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapFallbackToFile("index.html");
        }
    }
}