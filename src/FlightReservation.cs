record FlightReservation(string Id, string Name, string From, string To, DateTime Departure, DateTime Arrival);

record TravelReservation(string Id, FlightReservation Flight, HotelReservation Hotel);
