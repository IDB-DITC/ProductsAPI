using FastReport.Data;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductsAPI.Data;
using ProductsAPI.Models;
using ProductsAPI.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";




var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
						  //.AllowAnyOrigin()
						  //.WithOrigins("http://localhost:5289", "https://localhost:7115")
						  .WithOrigins("http://localhost:4200", "https://localhost:4200").AllowCredentials()
						  .AllowAnyHeader().AllowAnyMethod();
                      });
});
builder.Services.AddDbContext<ProductsAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsAPIContext") ?? throw new InvalidOperationException("Connection string 'ProductsAPIContext' not found.")));

// Add services to the container.





builder.Services.TryAddScoped<ITokenService, TokenService>();
builder.Services.TryAddScoped<ImageUploadService, ImageUploadService>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    // Support string to enum conversions
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole>()
    //.AddIdentityApiEndpoints<IdentityUser>(config =>
    //{
    //  //config.Tokens.AuthenticatorIssuer = JwtBearerDefaults.AuthenticationScheme;
    //    config.Password.RequiredLength = 4;
    //    config.Password.RequireNonAlphanumeric = false;
    //    config.Password.RequireDigit = false;
    //    config.Password.RequireUppercase = false;
    //    config.Password.RequireLowercase = false;
    //})

    .AddEntityFrameworkStores<ProductsAPIContext>();

//builder.Services.AddOptions<BearerTokenOptions>(JwtBearerDefaults.AuthenticationScheme).Configure(options =>
//{
//    options.BearerTokenExpiration = TimeSpan.FromSeconds(50000);
//    options.RefreshTokenExpiration = TimeSpan.FromSeconds(50000);
//});
//builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options => { 


//});
//builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => {


//});


var validIssuer = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidIssuer");
var validAudience = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidAudience");
var symmetricSecurityKey = builder.Configuration.GetValue<string>("JwtTokenSettings:SymmetricSecurityKey");


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        options.UseSecurityTokenValidators = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey)
            ),
            SaveSigninToken = true
        };
    });

//builder.Services
//        .AddAuthentication(options =>
//        {
//            //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//            //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//        })

//.AddJwtBearer(options =>
//{
//    options.UseSecurityTokenValidators = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        RequireExpirationTime = true,
//        AuthenticationType ="Bearer"
//    };
//    //options.TokenHandlers.Add(new BearerTokenHandler());
//})
;





builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });


    //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{
    //	In = ParameterLocation.Header,
    //	Name = "Authorization",
    //	Type = SecuritySchemeType.ApiKey
    //});
    //options.OperationFilter<SecurityRequirementsOperationFilter>();
});



FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

builder.Services.AddFastReport();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapSwagger().RequireAuthorization();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage();



app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseStatusCodePages();
app.UseStaticFiles();

//app.MapIdentityApi<IdentityUser>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseFastReport();



app.Run();
