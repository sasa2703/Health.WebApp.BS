using HealthManager.WebApp.BS.API.Extensions;
using HealthManager.WebApp.BS.Shared.Constants;
using HealthManager.WebApp.BS.Shared.Middleware;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureCors(builder.Configuration["AllowedOrigins"]);
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServices();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureAppOptions(builder.Configuration);
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureSwagger();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("InternalUser", policy =>
                      policy.RequireClaim(TokenClaims.UserCategory, UserCategory.Internal.ToString()));
    options.AddPolicy("EndUser", policy =>
                      policy.RequireClaim(TokenClaims.UserCategory, UserCategory.EndUser.ToString()));
    options.AddPolicy("InternalOrEndUser", policy =>
                      policy.RequireClaim(TokenClaims.UserCategory, UserCategory.Internal.ToString(), UserCategory.EndUser.ToString()));
});


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddControllers(config =>
{
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());

}).AddApplicationPart(typeof(HealthManager.WebApp.BS.Presentation.AssemblyReference).Assembly);

var app = builder.Build();
app.UseCustomExceptionHandler();

// Configure the HTTP request pipeline.

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Health Manager API");
});

app.MapControllers();

app.Run();

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
    .Services.BuildServiceProvider()
    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
    .OfType<NewtonsoftJsonPatchInputFormatter>().First();