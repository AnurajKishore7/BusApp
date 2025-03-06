export interface BusRoute {
    id: number;
    source: string;
    destination: string;
    estimatedDuration: string; 
    distance: number;
  }
  
  export interface NewBusRoute {
    source: string | null;
    destination: string | null;
    estimatedDuration: string; 
    distance: number;
  }
  
  export interface UpdateBusRoute {
    source: string | null;
    destination: string | null;
    estimatedDuration: string; 
    distance: number;
  }