using System.Text;
using EventApp.Application.Mapping;
using EventApp.Application.UseCases.Category;
using EventApp.Application.UseCases.Event;
using EventApp.Application.UseCases.Location;
using EventApp.Application.UseCases.Member;
using EventApp.Application.UseCases.RefreshToken;
using EventApp.Application.UseCases.User;
using EventApp.Core.Abstractions;
using EventApp.DataAccess;
using EventApp.DataAccess.Abstractions;
using EventApp.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventApp API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddAutoMapper(typeof(MappingCategory));
builder.Services.AddAutoMapper(typeof(MappingUser));
builder.Services.AddAutoMapper(typeof(MappingEvent));
builder.Services.AddAutoMapper(typeof(MappingMemberOfEvent));
builder.Services.AddAutoMapper(typeof(MappingLocation));

builder.Services.AddDbContext<EventAppDBContext>(
    options => { options.UseNpgsql(configuration.GetConnectionString(nameof(EventAppDBContext))); });

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<AddCategoryUseCase>();
builder.Services.AddScoped<DeleteCategoryUseCase>();
builder.Services.AddScoped<GetAllCategoriesUseCase>();
builder.Services.AddScoped<GetCategoryByIdUseCase>();
builder.Services.AddScoped<UpdateCategoryUseCase>();

builder.Services.AddScoped<AddLocationUseCase>();
builder.Services.AddScoped<DeleteLocationUseCase>();
builder.Services.AddScoped<GetAllLocationsUseCase>();
builder.Services.AddScoped<GetLocationByIdUseCase>();
builder.Services.AddScoped<UpdateLocationUseCase>();

builder.Services.AddScoped<AddMemberOfEvent>();
builder.Services.AddScoped<DeleteMemberOfEvent>();
builder.Services.AddScoped<DeleteMemberOfEventByEventIdAndUserId>();
builder.Services.AddScoped<GetAllMembersOfEventByEventId>();
builder.Services.AddScoped<GetAllMembersOfEventByUserId>();
builder.Services.AddScoped<GetMemberOfEventById>();
builder.Services.AddScoped<UpdateMemberOfEvent>();

builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<GetUserByIdUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<RegisterUserUseCase>();

builder.Services.AddScoped<DeleteRefreshToken>();
builder.Services.AddScoped<GetRefreshToken>();
builder.Services.AddScoped<Refresh>();


builder.Services.AddScoped<CreateEventUseCase>();
builder.Services.AddScoped<DeleteEventUseCase>();
builder.Services.AddScoped<GetEventByIdUseCase>();
builder.Services.AddScoped<GetEventsByFiltersUseCase>();
builder.Services.AddScoped<UpdateEventUseCase>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            
        });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventApp API V1"); });


app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowLocalhost3000");
app.MapControllers();

app.Run();