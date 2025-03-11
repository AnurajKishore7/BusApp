import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { ionLogoWhatsapp, ionMailOutline } from '@ng-icons/ionicons';
import { AuthService } from '../auth/auth.service';

interface Trip {
  tripId: number;
  operatorName: string;
  busNo: string;
  departure: string;
  arrival: string;
  startingFare: number;
  seatsAvailable: number;
  totalSeats: number;
  busType: string;
  departureLocation: string;
  arrivalLocation: string;
}

interface Passenger {
  seat: string;
  name: string;
  age: string;
  gender: string;
  isHandicapped: boolean;
}

interface BookingResponseDto {
  id: number;
  clientId: number;
  tripId: number;
  journeyDate: string;
  ticketCount: number;
  status: string;
  contact: string;
  clientName: string;
  source: string;
  destination: string;
  baseFare: number;
  gst: number;
  convenienceFee: number;
  totalAmount: number;
  paymentMethod: string;
  paymentStatus: string;
  ticketPassengers: TicketPassengerResponseDto[];
}


interface TicketPassengerResponseDto {
  Id: number;
  BookingId: number;
  PassengerName: string;
  Age: number;
  Gender: string;
  SeatNumber: string;
  IsHandicapped: boolean;
}

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule, FormsModule, NgIconComponent],
  providers: [
    provideIcons({ ionLogoWhatsapp, ionMailOutline })
  ],
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css']
})
export class BookingComponent implements OnInit {
  trip: Trip | null = null;
  selectedSeats: string[] = [];
  journeyDate: string = '';
  source: string = '';
  destination: string = '';

  passengers: Passenger[] = [];
  contactNumber: string = '';
  email: string = '';
  sendWhatsApp: boolean = true;
  sendEmail: boolean = false;

  baseFare: number = 0;
  gst: number = 0;
  convenienceFee: number = 0;
  totalFare: number = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private authService: AuthService
  ) {}

  ngOnInit() {
    
    this.route.queryParams.subscribe(params => {
      this.trip = params['trip'] ? JSON.parse(params['trip']) : null;
      this.selectedSeats = params['selectedSeats'] ? JSON.parse(params['selectedSeats']) : [];
      this.journeyDate = params['journeyDate'] || '';
      this.source = params['source'] || '';
      this.destination = params['destination'] || '';

      this.passengers = this.selectedSeats.map(seat => ({
        seat,
        name: '',
        age: '',
        gender: 'Male',
        isHandicapped: false
      }));

      this.calculateInitialFare();
    });
  }

  calculateInitialFare() {
    if (!this.trip) return;
    this.baseFare = this.trip.startingFare * this.selectedSeats.length;
    this.gst = this.baseFare * 0.06;
    this.convenienceFee = 10;
    this.totalFare = this.baseFare + this.gst + this.convenienceFee;
  }

  formatDate(dateStr: string): string {
    const date = new Date(dateStr);
    return date.toLocaleDateString('en-US', { day: '2-digit', month: 'short', year: 'numeric' });
  }

  getArrivalDate(): string {
    const departureDate = new Date(this.journeyDate);
    const [depHours, depMinutes] = this.trip!.departure.split(':').map(Number);
    const [arrHours, arrMinutes] = this.trip!.arrival.split(':').map(Number);

    const departureTime = departureDate.getTime() + depHours * 60 * 60 * 1000 + depMinutes * 60 * 1000;
    const arrivalTime = arrHours < depHours ? (arrHours + 24) * 60 * 60 * 1000 : arrHours * 60 * 60 * 1000 + arrMinutes * 60 * 1000;
    const arrivalDate = new Date(departureTime + (arrivalTime - (depHours * 60 * 60 * 1000 + depMinutes * 60 * 1000)));

    return arrivalDate.toLocaleDateString('en-US', { day: '2-digit', month: 'short', year: 'numeric' }) + ', ' + this.trip!.arrival;
  }

  calculateDuration(): string {
    const [depHours, depMinutes] = this.trip!.departure.split(':').map(Number);
    const [arrHours, arrMinutes] = this.trip!.arrival.split(':').map(Number);

    let depMinutesTotal = depHours * 60 + depMinutes;
    let arrMinutesTotal = arrHours * 60 + arrMinutes;
    if (arrMinutesTotal < depMinutesTotal) arrMinutesTotal += 24 * 60;

    const diffMinutes = arrMinutesTotal - depMinutesTotal;
    const hours = Math.floor(diffMinutes / 60);
    const minutes = diffMinutes % 60;
    return `${hours}h ${minutes}m`;
  }

  submitBooking() {
    if (!this.validateForm()) return;
  
    const bookingDto = {
      TripId: this.trip!.tripId,
      JourneyDate: this.journeyDate,
      TicketCount: this.selectedSeats.length,
      Contact: "9876543210",
      TicketPassengers: this.passengers.map(p => ({
        PassengerName: p.name,
        Age: p.age ? parseInt(p.age) : 0,
        Gender: p.gender.toUpperCase(),
        IsHandicapped: p.isHandicapped,
        SeatNumber: p.seat
      }))
    };
  
    const token = this.authService.getToken();
    if (!token) {
      alert('Please log in first.');
      this.router.navigate(['/']);
      return;
    }
  
    this.http
      .post<BookingResponseDto>('http://localhost:5205/api/booking', bookingDto, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        }
      })
      .subscribe({
        next: (response) => {
          this.baseFare = response.baseFare;
          this.gst = response.gst;
          this.convenienceFee = response.convenienceFee;
          this.totalFare = response.totalAmount;
  
          // Navigate to the Payment Page
          this.router.navigate(['/payment'], {
            queryParams: {
              bookingId: response.id,
              totalAmount: this.totalFare
            }
          });
        },
        error: (err: HttpErrorResponse) => {
          console.error('Booking Error:', err);
          console.error('Error Status:', err.status);
          console.error('Error Body:', JSON.stringify(err.error, null, 2));
          alert('Failed to submit booking. Error: ' + (err.error?.message || err.message));
        }
      });
  }

  validateForm(): boolean {
    if (!this.contactNumber || this.contactNumber.length !== 10) {
      alert('Please enter a valid 10-digit mobile number.');
      return false;
    }

    for (const passenger of this.passengers) {
      if (!passenger.name || !passenger.age || !passenger.gender) {
        alert('Please fill in all passenger details.');
        return false;
      }
      const age = parseInt(passenger.age);
      if (isNaN(age) || age <= 0) {
        alert('Please enter a valid age for all passengers.');
        return false;
      }
    }

    if (this.sendEmail && (!this.email || !this.email.includes('@'))) {
      alert('Please enter a valid email address.');
      return false;
    }

    return true;
  }
}