using BlogEngineSGC.CommonClasses;
using BlogEngineSGC.Services;
using WebMarkupMin.AspNetCore8;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddW3CLogging(logging =>
{
    // Log all W3C fields
    logging.LoggingFields = W3CLoggingFields.All;

    ////logging.FileSizeLimit = 5 * 1024 * 1024;
    ////logging.RetainedFileCountLimit = 2;
    ////logging.FileName = "MyLogFile";
    ////logging.LogDirectory = @"C:\logs";
    //logging.FlushInterval = TimeSpan.FromSeconds(2);
});

// Add custom services to the container.
builder.Services.AddSingleton<IUserService, BlogUserService>();
builder.Services.AddSingleton<IBlogService, FileBlogService>();
builder.Services.AddSingleton<IBlogService, FileBlogDataService>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();


builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.Configure<BlogsSettings>(
    builder.Configuration.GetSection("blog"));

// Cookie authentication.
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
        options =>
        {
            options.LoginPath = "/login/";
            options.LogoutPath = "/logout/";
        });

// HTML minification (https://github.com/Taritsyn/WebMarkupMin)
builder.Services
    .AddWebMarkupMin(
        options =>
        {
            options.AllowMinificationInDevelopmentEnvironment = true;
            options.DisablePoweredByHttpHeaders = true;
        })
    .AddHtmlMinification(
        options =>
        {
            options.MinificationSettings.RemoveOptionalEndTags = false;
            options.MinificationSettings.WhitespaceMinificationMode = WhitespaceMinificationMode.Safe;
        });

builder.Services
     .AddWebMarkupMin(options =>
     {
         options.AllowMinificationInDevelopmentEnvironment = true;
         options.AllowCompressionInDevelopmentEnvironment = true;
         // options.DisableMinification = !EngineContext.Current.Resolve<CommonSettings>().EnableHtmlMinification;
         options.DisableCompression = true;
         options.DisablePoweredByHttpHeaders = true;
     })
     .AddHtmlMinification(options =>
     {
         options.CssMinifierFactory = new NUglifyCssMinifierFactory();
         options.JsMinifierFactory = new NUglifyJsMinifierFactory();
     })
     .AddXmlMinification(options =>
     {
         var settings = options.MinificationSettings;
         settings.RenderEmptyTagsWithSpace = true;
         settings.CollapseTagsWithoutContent = true;
     });

var app = builder.Build();

// Configure the HTTP request pipeline. currently only development
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

/*else
{
    app.UseWebMarkupMin();
}
*/
app.UseStaticFiles();

app.UseWebMarkupMin();

app.UseRouting();

app.UseW3CLogging();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();





