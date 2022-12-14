using EasyPollAPI.DbInitializer;
using EasyPollAPI.Hubs;
using EasyPollAPI.Models;
using EasyPollAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddScoped<PollGameService>();
builder.Services.AddScoped<TempUserService>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<DbInitializer>();

builder.Services.AddDbContext<EasyPollContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin 
    .AllowCredentials());

/*
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<PollGameHub>("/PollGameSocket");
});
*/
app.MapHub<PollGameHub>("/PollGameSocket");
app.MapControllers();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    serviceScope.ServiceProvider.GetService<DbInitializer>().Seed();
}

app.Run();
