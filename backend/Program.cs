using Microsoft.EntityFrameworkCore;
using dotenv.net;
using System.Diagnostics;
using AdminApi;
using AdminApi.Models;

// Load environment variables
DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] {"./.env"}));

var envVars = DotEnv.Read();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Load environment variables for connections
string? devDbHost = envVars["DEV_DB_HOST"];
string? devDbPort = envVars["DEV_DB_PORT"];
string? devDbUsername = envVars["DEV_DB_USERNAME"];
string? devDbPassword = envVars["DEV_DB_PASSWORD"];
string? devDbName = envVars["DEV_DB_NAME"];

string? testDbHost = envVars["TEST_DB_HOST"];
string? testDbPort = envVars["TEST_DB_PORT"];
string? testDbUsername = envVars["TEST_DB_USERNAME"];
string? testDbPassword = envVars["TEST_DB_PASSWORD"];
string? testDbName = envVars["TEST_DB_NAME"];

string? prodDbHost = envVars["PROD_DB_HOST"];
string? prodDbPort = envVars["PROD_DB_PORT"];
string? prodDbUsername = envVars["PROD_DB_USERNAME"];
string? prodDbPassword = envVars["PROD_DB_PASSWORD"];
string? prodDbName = envVars["PROD_DB_NAME"];

string? adminDbHost = envVars["ADMIN_DB_HOST"];
string? adminDbPort = envVars["ADMIN_DB_PORT"];
string? adminDbUsername = envVars["ADMIN_DB_USERNAME"];
string? adminDbPassword = envVars["ADMIN_DB_PASSWORD"];
string? adminDbName = envVars["ADMIN_DB_NAME"];

Console.WriteLine(Directory.GetCurrentDirectory());

Console.ReadLine();

// Add connection strings to configuration
builder.Configuration["ConnectionStrings:DevConnection"] =
    $"Host={devDbHost};Port={devDbPort};Username={devDbUsername};Password={devDbPassword};Database={devDbName}";

builder.Configuration["ConnectionStrings:TestConnection"] =
    $"Host={testDbHost};Port={testDbPort};Username={testDbUsername};Password={testDbPassword};Database={testDbName}";

builder.Configuration["ConnectionStrings:ProdConnection"] =
    $"Host={prodDbHost};Port={prodDbPort};Username={prodDbUsername};Password={prodDbPassword};Database={prodDbName}";

builder.Configuration["ConnectionStrings:AdminConnection"] =
    $"Host={adminDbHost};Port={adminDbPort};Username={adminDbUsername};Password={adminDbPassword};Database={adminDbName}";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "development",
    policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});

builder.Services.AddHttpLogging(o => { });


builder.Services.AddControllers();
builder.Services.AddDbContext<AdminContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DevConnection"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (args.Length == 1)
{
    if (args.Contains("migrate"))
    {
        return 0;
    } else if (args.Contains("seed"))
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AdminContext>();
            Seeder.Seed(context, 0);
        }
        return 0;
    } else if (args.Contains("completeSeed"))
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AdminContext>();
            Seeder.Seed(context, 1);
        }
        return 0;
    }
}

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("development");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

return 0;
