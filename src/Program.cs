using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

List<FlightReservation> flights =
[
    new ("55623185-beb2-49a3-8c69-747605b7ac60", "Flight 1", "SEA", "JFK", DateTime.Now, DateTime.Now.AddDays(1)),
    new ("91daf04a-8987-483a-9549-ab7cb708be05", "Flight 2", "LAX", "ORD", DateTime.Now, DateTime.Now.AddDays(2)),
    new ("b036a46f-a60c-4927-b0ce-cbfc9e87f863", "Flight 3", "DFW", "MIA", DateTime.Now, DateTime.Now.AddDays(3)),
    new ("f6358742-f39b-4a99-93da-911c2d851065", "Flight 4", "ATL", "DEN", DateTime.Now, DateTime.Now.AddDays(4)),
    new ("f41ad366-ae5a-48f0-a1ed-6676c9449af1", "Flight 5", "SFO", "BOS", DateTime.Now, DateTime.Now.AddDays(5))
];

List<HotelReservation> hotels =
[
    new ("b034aca7-6ed8-46e1-b82e-b9280483d5b0", "Hotel 1", "123 Main St", DateTime.Now, DateTime.Now.AddDays(1)),
    new ("5100272d-4b50-4431-8c85-ecafc3e729ea", "Hotel 2", "456 Elm St", DateTime.Now, DateTime.Now.AddDays(2)),
    new ("8dff740e-a954-4c68-b643-a7a520764ce5", "Hotel 3", "789 Oak St", DateTime.Now, DateTime.Now.AddDays(3)),
    new ("1ee120d3-5a88-4f46-9689-20a59f4f6ebc", "Hotel 4", "321 Pine St", DateTime.Now, DateTime.Now.AddDays(4)),
    new ("960c74ea-7116-44e1-a735-8438af1f65b6", "Hotel 5", "654 Maple St", DateTime.Now, DateTime.Now.AddDays(5))
 ];

// Custom middleware to set a custom status code.
app.Use(async (context, next) =>
{
    if (context.Request.Headers.ContainsKey("x-custom-status"))
    {
        context.Response.StatusCode = int.Parse(context.Request.Headers["x-custom-status"]);
        await context.Response.WriteAsync("Custom status code set.");

        return;
    }
    await next();
});

app.MapGet("/", () => "Hello World!");

// Flight
app.MapGet("/api/reservation/flights", () => flights);

app.MapGet("/api/reservation/flight/{id}", (string id) =>
{
    // Perform some logic to get a flight reservation
    app.Logger.LogInformation($"Getting reservation {id}.");
    FlightReservation? flightReservation = flights.FirstOrDefault(f => f.Id == id);
    return flightReservation;

});

app.MapPost("/api/reservation/flight", (FlightReservation flight) =>
{
    // Perform some logic to create a new flight reservation
    string id = Guid.NewGuid().ToString();
    var f = new FlightReservation(id, flight.Name, flight.From, flight.To, flight.Departure, flight.Arrival);
    flights.Add(f);
    return Results.Created($"/api/reservation/flight/{id}", f);
});

app.MapDelete("/api/reservation/flight/{id}", (string id) =>
{
    app.Logger.LogInformation($"Deleting reservation {id}");
    FlightReservation? flightReservation = flights.FirstOrDefault(f => f.Id == id);
    if (flightReservation == null)
    {
        return Results.NotFound($"Reservation {id} not found.");
    }
    else
    {
        flights.Remove(flightReservation);
    }

    return Results.Ok($"Deleting reservation {id}");
});

// Hotel
app.MapGet("/api/reservation/hotels", () => hotels);

app.MapGet("/api/reservation/hotel/{id}", (string id) =>
{
    app.Logger.LogInformation($"Getting reservation {id}.");

    HotelReservation? hotelReservation = hotels.FirstOrDefault(h => h.Id == id);

    return hotelReservation;
});

app.MapPost("/api/reservation/hotel", (HotelReservation hotel) =>
{
    // Perform some logic to create a new hotel reservation
    string id = Guid.NewGuid().ToString();

    // Return a HTTP Created result with the newly created reservation
    return Results.Created($"/api/reservation/hotel/{id}", new HotelReservation(id, hotel.Name, hotel.Address, hotel.CheckIn, hotel.CheckOut));
});


app.MapDelete("/api/reservation/hotel/{id}", (string id) =>
{
    app.Logger.LogInformation($"Deleting reservation {id}.");
    return Results.Ok($"Deleting reservation {id}.");
});


app.MapPost("/api/reservation/confirmation", (TravelReservation reservation) =>
{
    // Perform some logic to confirm the reservation
    app.Logger.LogInformation($"Confirming reservation for {reservation.Flight.Name} and {reservation.Hotel.Name}.");
    return Results.Ok($"Reservation confirmed for {reservation.Flight.Name} and {reservation.Hotel.Name}.");
});


app.Run();
