using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapPost("/flight/reservation", () => Guid.NewGuid());
app.MapDelete("/flight/reservation/{id}", (string id) => $"Working on reservation {id}");
app.MapGet("/hotel", () => "Reservation complete");
app.MapGet("/hotel/{id}", (string id) => $"Hotel reservation {id}.");


app.Run();
