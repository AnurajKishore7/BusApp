export interface ClientRegister {
    name: string;
    email: string;
    password: string;
    dob: string; // ISO format (e.g., "1990-01-01")
    gender: string;
    contact: string;
    isHandicapped: boolean;
  }