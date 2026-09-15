var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDefaultFiles(); // Serves index.html by default
app.UseStaticFiles();  // Serves files from wwwroot

app.UseRouting();

app.MapControllers();

app.Run("http://localhost:5000");

