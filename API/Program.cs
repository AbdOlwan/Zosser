using API.Services;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy => policy
            // أضف النطاقات التي تريد السماح لها هنا
            .WithOrigins("http://localhost:8080", "https://zosser-app.netlify.app", "http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader() // هذا هو الجزء الأهم لحل مشكلة الـ headers
            .AllowCredentials() // إذا كنت تستخدم cookies أو authentication headers
    );
});



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();



builder.Services
    .ConfigureDbContext(builder.Configuration)
    .ConfigureIdentity()
    .ConfigureHttpContext()
    .ConfigureAuthentication(builder.Configuration) // إضافة التوثيق
    .ConfigureAuthorization() // إضافة التصريح
    .ConfigureBusinessServices()
    .ConfigureMapster()
    .ConfigureSwagger();

var app = builder.Build();

// تكوين خطوط الأنابيب
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");


app.UseAuthentication(); 
app.UseAuthorization(); 
app.MapControllers();

// تطبيق الترحيل التلقائي لقاعدة البيانات
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.Run();