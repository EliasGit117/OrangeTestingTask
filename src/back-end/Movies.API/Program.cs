using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Movies.API.Common;
using Movies.API.Common.Jwt;
using Movies.Application;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddApplicationServices(builder.Configuration);
services.AddSwaggerGen(options =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Description = "Json Web Token for authorization. Write: 'Bearer {your token}'",
        Name = HeaderNames.Authorization,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    };
    options.AddSecurityDefinition(securityScheme.Scheme, securityScheme);

    var requirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = securityScheme.Scheme
                },
                Scheme = "Bearer",
                Name = securityScheme.Scheme,
                In = securityScheme.In
            },
            new List<string>()
        }
    };

    options.AddSecurityRequirement(requirement);
});

services.AddJwtAuth(builder.Configuration);
// services.AddAuthorization();


var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.InjectStylesheet("/dark-swagger.css"); });
}

app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();