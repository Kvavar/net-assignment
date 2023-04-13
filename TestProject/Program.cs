using Serilog;
using Serilog.Events;
using Work.ApiModels;
using Work.Database;
using Work.Implementation;
using Work.Interfaces;
using Work.Mappers;

// only log to console for simplicity
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    // Add services to the container.

    // singleton to test via Swagger
    builder.Services.AddSingleton<MockDatabase>();
    builder.Services.AddScoped<IMapper<User, UserModelDto>, UserMapper>();
    builder.Services.AddScoped<IRepository<User, Guid>, UserRepository>();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Exception while starting the application.");
}
finally
{
    Log.CloseAndFlush();
}

