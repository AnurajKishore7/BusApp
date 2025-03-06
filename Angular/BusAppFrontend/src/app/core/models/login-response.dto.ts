export interface LoginResponse {
  name: string;
  email: string;
  role: 'Admin' | 'TransportOperator' | 'Client';
  message: string;
  token: string | null;
}