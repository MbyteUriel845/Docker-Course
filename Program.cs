using DemoDockerAPI.Data;
using DemoDockerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Bogus;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? "Server=sqlserver,1433;Database=DemoDB;User Id=sa;Password=Admin123!;TrustServerCertificate=True;";

builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment() || true)
{
}

app.UseAuthorization();
app.MapControllers();

// Ensure DB is reachable and seeded with retries
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var db = services.GetRequiredService<AppDbContext>();

    var maxAttempts = 30;
    var attempt = 0;
    while (attempt < maxAttempts)
    {
        try
        {
            logger.LogInformation("Attempting database ensure created (attempt {Attempt})", attempt+1);
            // Use EnsureCreated to avoid requiring dotnet-ef migrations in the build pipeline.
            db.Database.EnsureCreated();
            if (!db.Products.Any())
            {
                // Seed 20 products using Bogus
                var faker = new Faker<Product>()
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Description, f => f.Commerce.ProductAdjective() + " " + f.Commerce.ProductMaterial())
                    .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(5, 200)));

                var products = faker.Generate(20);
                db.Products.AddRange(products);
                db.SaveChanges();
                logger.LogInformation("Seeded {Count} products", products.Count);
            }
            break;
        }
        catch (Exception ex)
        {
            attempt++;
            logger.LogWarning(ex, "Database not ready yet. Waiting 2s before retry... (attempt {Attempt} of {Max})", attempt, maxAttempts);
            await Task.Delay(2000);
        }
    }

    if (attempt >= maxAttempts)
    {
        logger.LogError("Could not connect to the database after {Max} attempts.", maxAttempts);
    }
}

app.Run();