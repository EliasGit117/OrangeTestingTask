using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Movies.API.Common.Jwt;

public static class ConfigureJwtAuth
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfigurationSection = configuration.GetSection("JwtConfiguration");
        var jwtConfiguration = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();
        
        services.Configure<JwtConfiguration>(jwtConfigurationSection);

        
        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var key = Encoding.UTF8.GetBytes(jwtConfiguration!.SecretKey);

                o.MapInboundClaims = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtConfiguration.ValidateIssuer,
                    ValidateAudience = jwtConfiguration.ValidateAudience,
                    ValidateLifetime = jwtConfiguration.ValidateLifetime,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfiguration.Issuer,
                    ValidAudience = jwtConfiguration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        return services;
    }
}