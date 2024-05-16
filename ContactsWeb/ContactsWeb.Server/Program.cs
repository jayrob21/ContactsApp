using ContactsWeb.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.AddServerHeader = false;
    
//    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
//});
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error/");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("Referrer-Policy", "no-referrer");
    context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", "none");
    context.Response.Headers.Append("Permissions-Policy", 
        "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
    context.Response.Headers.Server = "" ;


    List<string> blackListedMethods = ["OPTIONS", "PATCH", "TRACE"];
    if (blackListedMethods.Contains(context.Request.Method))
    {
        context.Response.StatusCode = 405;
        return;
    }
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        Utilities.LogError(ex);
        context.Response.Clear();
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(ex.Message);

    }
});

app.MapControllers();

app.MapFallbackToFile("/index.html");

ContactDsJsonManager.CreateDsFile();

app.Run();