record FlightReservation(string Id, string Name, string From, string To, DateTime Departure, DateTime Arrival);

record HotelReservation(string Id, string Name, string Address, DateTime CheckIn, DateTime CheckOut);

record TravelReservation(string Id, FlightReservation Flight, HotelReservation Hotel);
