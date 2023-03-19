using MongoDB.Driver.Core.Configuration;
using OnlineStore;
using OnlineStore.Services;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
builder.Services.AddScoped<CategoriesService>();
builder.Services.AddScoped<ProductsService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<OrdersService>();
builder.Services.AddScoped<BlobService>();
builder.Services.AddScoped<SessionsService>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();

var appSettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
var sinkOpts = new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true };
var logger = new LoggerConfiguration()
    .WriteTo.MSSqlServer(
        connectionString: ConnectionManager.CreateConnectionStringSQL(builder.Configuration),
        sinkOptions: sinkOpts,
        appConfiguration: appSettings
    ).CreateLogger();
builder.Host.UseSerilog(logger);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors();
app.Run();
