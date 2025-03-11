// src/app/core/models/trip-results.dto.ts
export interface RawTripResult {
  tripId: number;
  departure: string;
  arrival: string;
  operatorName: string;
  busNo: string;
  pricePerSeat: number;
  seatsAvailable: number;
  totalSeats: number;
  busType: string;
}

export interface TripDetailsResponse {
  id: number;
  busRouteId: number;
  busId: number;
  departureTime: string;
  arrivalTime: string;
  price: number;
  source: string;
  destination: string;
  journeyDate: string;
  availableSeats: string[];
}

export interface TimeSpan {
  hours: number;
  minutes: number;
  toString(): string;
}

export interface TripResults {
  tripId: number;
  operatorName: string;
  busNo: string;
  departure: TimeSpan;
  arrival: TimeSpan;
  startingFare: number;
  seatsAvailable: number;
  totalSeats: number;
  busType: string;
  departureLocation?: string;
  arrivalLocation?: string;
  availableSeats?: string[];
}

export function parseTripResult(raw: RawTripResult, source: string, destination: string): TripResults {
  const parseTime = (timeStr: string): TimeSpan => {
    const [hours, minutes] = timeStr.split(':').map(Number);
    return {
      hours,
      minutes,
      toString: () => `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`
    };
  };

  return {
    tripId: raw.tripId,
    operatorName: raw.operatorName,
    busNo: raw.busNo,
    departure: parseTime(raw.departure),
    arrival: parseTime(raw.arrival),
    startingFare: raw.pricePerSeat,
    seatsAvailable: raw.seatsAvailable,
    totalSeats: raw.totalSeats,
    busType: raw.busType,
    departureLocation: source,
    arrivalLocation: destination,
    availableSeats: undefined
  };
}