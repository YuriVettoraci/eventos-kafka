using Microsoft.AspNetCore.Mvc;
using webHook.ProducerApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ProducerTeste>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("publica-eventos", async ([FromServices] ProducerTeste producer, CancellationToken cancellationToken) =>
{
    await producer.PublicaEventoAsync(cancellationToken);

    return "Evento enviado!";
}).WithOpenApi();

app.UseAuthorization();

app.MapControllers();

app.Run();
