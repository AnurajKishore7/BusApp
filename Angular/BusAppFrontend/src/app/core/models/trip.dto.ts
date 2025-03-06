export interface Trip {
    id: number;
    busRouteId: number;
    busId: number;
    departureTime: string; 
    arrivalTime: string;   
    price: number;
  }
  
  export interface NewTrip {
    busRouteId: number;
    busId: number;
    departureTime: string; 
    arrivalTime: string;   
    price: number;
  }
  
  export interface UpdateTrip {
    busRouteId: number;
    busId: number;
    departureTime: string; 
    arrivalTime: string;   
    price: number;
  }