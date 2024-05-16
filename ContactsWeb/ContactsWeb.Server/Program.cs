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