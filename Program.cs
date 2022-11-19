using Hangfire;
using Hangfire.MemoryStorage;
using IpaddressesWebAPI.DBFolder;
using IpaddressesWebAPI.Handlers;
using IpaddressesWebAPI.Jobs;
using IpaddressesWebAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//data context
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Transient);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//memory helpers
builder.Services.AddMemoryCache();

//useful resources
builder.Services.AddTransient<MainHandler>();
builder.Services.AddTransient<CountryRepository>();
builder.Services.AddTransient<IPAddressesRepository>();
builder.Services.AddTransient<MainRepository>();
builder.Services.AddTransient<DatabaseRefreshTaskManager>();
builder.Services.AddTransient<DatabaseCronJob>();

//for cron job
builder.Services.AddSingleton<RecurringJobManager>();

//config hangfire
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseDefaultTypeSerializer()
    .UseMemoryStorage());

builder.Services.AddHangfireServer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

var scopeeee = app.Services.CreateScope();
var databaseRefreshTask = scopeeee.ServiceProvider.GetService<DatabaseRefreshTaskManager>();
databaseRefreshTask.InsertTask();
app.Run();
