using Building_Blocks.Contracts;
using LedgerService;
using LedgerService.Consumers;
using LedgerService.Setup;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.UseMessageRetry(r =>
        {
            r.Interval(3, TimeSpan.FromSeconds(5));
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddApiConfig(builder.Configuration);

var host = builder.Build();

host.Services.EnsureDatabaseCreated();

host.Run();
