export interface NewBus {
  busNo: string | null; 
  busType: string | null; 
  totalSeats: number;
}

export interface UpdateBus {
  busNo: string | null; 
  busType: string | null; 
  totalSeats: number; 
}