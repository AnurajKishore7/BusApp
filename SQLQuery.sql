select * from users
select * from clients
select * from TransportOperators where id = 1

select * from busroutes where id = 1

select * from buses where TotalSeats = 40

select Distinct BusType from buses where OperatorId = 11

select * from trips where BusRouteId = 1

update trips
set price = 950
where id = 3

