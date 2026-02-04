using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using SZL_Backend;
using SZL_Backend.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SZL Backend API",
        Version = "v1",
        Description = "API documentation for SZL Backend"
    });
    c.EnableAnnotations();
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'DefaultConnection' is not set. " +
        "If you are on your personal device run dotnet user-secrets set \"ConnectionStrings:DefaultConnectionString\" \"<conn>\" ");
}

builder.Services.AddDbContext<SZLDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SZL Backend API v1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseCors(corsPolicyBuilder => corsPolicyBuilder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();