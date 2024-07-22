using HC_Sandbox;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services
            .AddHttpContextAccessor()
            .AddGraphQLServer()
            .InitializeOnStartup()
            .RegisterDbContext<EFDbContext>()
            .AddHC_SandboxTypes()
            .AddType<Toyota>()
            .AddType<Nissan>()
            .AddGlobalObjectIdentification();


builder.Services.AddDbContext<EFDbContext>((cfg) =>
{
    cfg.UseLazyLoadingProxies();
    cfg.UseSqlite($"Data Source=domain.db");
    cfg.EnableSensitiveDataLogging(false);
});

builder.Services.AddCors();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EFDbContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    EFDbContext.Populate(context);
}

app.UseCors(corsPolicyBuilder =>
{
    corsPolicyBuilder
    .AllowAnyMethod()
    .AllowAnyHeader();

    if (app.Environment.IsDevelopment())
    {
        corsPolicyBuilder
        .AllowAnyOrigin();
    }
    else
    {
        corsPolicyBuilder
        .WithOrigins(
            "https://127.0.0.1",
            "https://localhost",
            "https://localhost:8000",

            "http://127.0.0.1",
            "http://localhost",
            "http://localhost:8000")
        .AllowCredentials();
    }
});

app.MapGraphQL();
app.Run();
