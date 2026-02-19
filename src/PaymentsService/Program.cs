using Building_Blocks.Contracts;
using BuildingBlocks.Contracts;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/payments", async (
    IPublishEndpoint publish,
    CreatePaymentRequest request) =>
{
    var paymentId = Guid.NewGuid();

    await publish.Publish(new PaymentCreated(
        paymentId,
        request.AccountId,
        request.Amount,
        DateTime.UtcNow));

    return Results.Accepted($"/payments/{paymentId}");
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
