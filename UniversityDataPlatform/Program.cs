using Microsoft.EntityFrameworkCore;
using UniversityDataPlatform.Components;
using UniversityDataPlatform.Data;
using UniversityDataPlatform.Services;
using UniversityDataPlatform.Repositories;
using UniversityDataPlatform.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

// 0. ÇEVRESEL DEĞİŞKENLER (.env)
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
if (!File.Exists(envPath))
{
    envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
}
if (File.Exists(envPath))
{
    foreach (var line in File.ReadAllLines(envPath))
    {
        var trimmedLine = line.Trim();
        if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#")) continue;
        var parts = trimmedLine.Split('=', 2);
        if (parts.Length == 2)
        {
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
        }
    }
}

var builder = WebApplication.CreateBuilder(args);

// --- 1. VERİ TABANI & FACTORY AYARI (KESİN ÇÖZÜM) ---
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

// Factory kurulumunu yapıyoruz. Bu, NavMenu'deki DbFactory için gerekli.
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Repository'lerin (IDatasetRepository vb.) hata vermeden çalışması için 
// standart DbContext isteğini Factory üzerinden karşılıyoruz.
builder.Services.AddScoped(p =>
    p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());


// --- 2. KİMLİK DOĞRULAMA & OTURUM YÖNETİMİ ---
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthentication();


// --- 3. REPOSITORY KAYITLARI (VERİ ERİŞİM) ---
builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDatasetRepository, DatasetRepository>();


// --- 4. SERVİS KAYITLARI (İŞ MANTIĞI) ---
builder.Services.AddHttpClient();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AcademicSearchService>();


// --- 5. BLAZOR BİLEŞEN AYARLARI ---
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// --- AUTOMATIC DATABASE MIGRATIONS ---
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database Migration Error: {ex.Message}");
    }
}

// --- 6. HTTP HAT VE GÜVENLİK AYARLARI ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();