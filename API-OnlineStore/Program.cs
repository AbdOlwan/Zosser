using API_OnlineStore.Extensions;
using BLL_OnlineStore.Mapping;
using DAL_OnlineStore.Configurations.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// 1) Configure your own dependencies (DAL, BLL, DbContext, etc.)
builder.Services.ConfigureDependencies(builder.Configuration);
// 1.1 سجل IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
// 2) Add Controllers + JSON converters في استدعاء موحد
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // ثبت المحولات مرة واحدة
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });


// 3) AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

// 4) Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // لو حبّيت تضيف XML Comments بعد تفعيلها في .csproj
    // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// في Startup.cs أو Program.cs
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("ar");
    options.SupportedCultures = new[] { new CultureInfo("ar"), new CultureInfo("en") };
    options.SupportedUICultures = new[] { new CultureInfo("ar"), new CultureInfo("en") };
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider());
});


var app = builder.Build();

// 5) Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//InvalidOperationException: Unable to resolve service for type 'DAL_OnlineStore.Repositories.Interfaces.IUnitOfWork' while attempting to activate 'BLL_OnlineStore.Services.OrderBusServices.OrderItemService'.
app.UseHttpsRedirection();
app.UseRequestCulture();
//app.UseRouting();
// لو عندك Authentication
app.UseAuthentication();
app.UseAuthorization();

// 6) ثبت الـ Controllers Routing
app.MapControllers();

app.Run();
