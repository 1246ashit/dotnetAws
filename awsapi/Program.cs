using awsapi.Controllers;
using awsapi.Entities;
using awsapi.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var _config = builder.Configuration;
//cros
var awsApi = "awsApi";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: awsApi,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
//

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//jwt
builder.Services.AddOptions<JwtSetEntity>()
    .BindConfiguration(JwtSetEntity.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<JwtService>();
builder.Services.AddJwtAuthentication(builder.Configuration);
//
builder.Services.AddSwaggerGen(opt =>
{
    //let swager can put JwtToken 
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
    //
});

builder.Services.AddHttpClient();
//服務
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<IUserService, UserService>();
//
// 控制器
builder.Services.AddControllers();
//
var app = builder.Build();

// Configure the HTTP request pipeline.
var swaggerEnabled = builder.Configuration.GetValue<bool>("Swagger:Enable");
if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(awsApi);//cros

//jwt
app.UseAuthentication(); // 身份驗證
app.UseAuthorization(); // 驗證授權 (原本應該就有這行，注意不要重複加入)
//

app.MapControllers(); // 這裡配置路由以使用控制器
app.Run();
