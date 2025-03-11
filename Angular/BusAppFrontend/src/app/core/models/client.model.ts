export interface Client {
    id: number;
    name: string;
    email?: string;
    dob?: string;
    gender?: string;
    contact?: string;
    isHandicapped: boolean;
    isDeleted: boolean;
  }