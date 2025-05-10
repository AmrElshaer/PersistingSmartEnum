using Microsoft.EntityFrameworkCore;
using PersistingSmartEnum;
using PersistingSmartEnum.Entities;
using PersistingSmartEnum.Enums;
using PersistingSmartEnum.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.MapPost("/orders/create", async (CreateOrder request,ApplicationDbContext context) =>
{
    var order = Order.Create(Guid.NewGuid(), request.CustomerName, request.Address);
    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();
    return order.Id;
});
app.MapPut("/orders/{uuid:guid}/shipped", async (Guid uuid,ApplicationDbContext context) =>
{
    var order = await context.Orders.FirstOrDefaultAsync(o=>o.Id==uuid);
    if (order is null)
    {
        return Results.NotFound();
    }
    order.MarkAsShipped();
    await context.SaveChangesAsync();
    return Results.Ok(order.Id);
});
app.MapPut("/orders/{uuid:guid}/delivered", async (Guid uuid, ApplicationDbContext context) =>
{
    var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == uuid);
    if (order is null)
    {
        return Results.NotFound();
    }

    order.MarkAsDelivered();
    await context.SaveChangesAsync();
    return Results.Ok(order.Id);
});
app.MapGet("/orders/{uuid:guid}/order", async (Guid uuid, ApplicationDbContext context,ICurrentUserService currentUser) =>
{
    var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == uuid);
    if (order is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(OrderDto.Map(order,currentUser.Language) );
});
app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();
public record CreateOrder(string CustomerName, string Address);

public record OrderDto(string CustomerName, string Address, string StatusName)
{
    public static OrderDto Map(Order order,LanguageEnum languageIso)
    {
        var statusName = order.Status.GetLocalizations().FirstOrDefault(l => l.Language == languageIso)
            ?.Name ?? order.Status.Name;
        return new OrderDto(order.CustomerName,order.Address,statusName);
    }
};
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}