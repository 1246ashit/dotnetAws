using awsapi.Controllers;
var builder = WebApplication.CreateBuilder(args);

//cros
var awsApi="awsApi";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: awsApi,
                      policy  =>
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
builder.Services.AddSwaggerGen();

// 控制器
builder.Services.AddControllers();
//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(awsApi);//cros

app.MapControllers(); // 這裡配置路由以使用控制器
app.Run();
