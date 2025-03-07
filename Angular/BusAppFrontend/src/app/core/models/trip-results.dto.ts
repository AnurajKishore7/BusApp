// Interface for raw API response
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

//TripResults interface
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
}

export interface TimeSpan {
  hours: number;
  minutes: number;
  toString(): string;
}

// Utility function to parse RawTripResult into TripResults
export function parseTripResult(raw: RawTripResult): TripResults {
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
      departureLocation: undefined, 
  };
}