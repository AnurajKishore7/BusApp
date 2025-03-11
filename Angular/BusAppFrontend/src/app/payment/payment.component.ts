// payment.component.ts
import { Component, OnInit, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../auth/auth.service';
import { saveAs } from 'file-saver';
import { Location } from '@angular/common';

interface PaymentResponseDto {
  id: number;
  bookingId: number;
  totalAmount: number;
  paymentMethod: string;
  status: string;
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
  id: number;
  bookingId: number;
  passengerName: string;
  age: number;
  gender: string;
  seatNumber: string;
  isHandicapped: boolean;
}

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit {
  bookingId: number = 0;
  totalAmount: number = 0;
  paymentMethod: string = 'UPI';
  paymentMethods: string[] = ['UPI', 'Credit Card', 'Debit/ATM Card', 'Net Banking'];
  paymentId: number = 0;
  isProcessing: boolean = false;
  isLoading: boolean = false;
  paymentSuccess: boolean = false;
  bookingDetails: BookingResponseDto | null = null;

  constructor(
    private route: ActivatedRoute,
    public router: Router,
    private http: HttpClient,
    private authService: AuthService,
    private location: Location
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.bookingId = params['bookingId'] ? parseInt(params['bookingId']) : 0;
      this.totalAmount = params['totalAmount'] ? parseFloat(params['totalAmount']) : 0;
      this.fetchBookingDetails();
    });
  }

  @HostListener('window:popstate', ['$event'])
  onPopState(event: Event) {
    if (this.paymentSuccess) {
      this.router.navigate(['/']);
    }
  }

  fetchPaymentId() {
    this.isLoading = true;
    const token = this.authService.getToken();
    if (!token) {
      alert('Please log in first.');
      this.router.navigate(['/']);
      this.isLoading = false;
      return;
    }

    this.http
      .get<PaymentResponseDto>(`http://localhost:5205/api/payment/booking/${this.bookingId}`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        }
      })
      .subscribe({
        next: (response) => {
          this.paymentId = response.id;
          this.isLoading = false;
        },
        error: (err: HttpErrorResponse) => {
          console.error('Error fetching payment ID:', err);
          alert('Failed to fetch payment details. Error: ' + (err.error?.message || err.message));
          this.isLoading = false;
        }
      });
  }

  confirmPayment() {
    if (!this.paymentId || !this.validatePayment()) return;

    this.isProcessing = true;
    const token = this.authService.getToken();
    if (!token) {
      alert('Please log in first.');
      this.router.navigate(['/']);
      return;
    }

    this.http
      .post<PaymentResponseDto>(`http://localhost:5205/api/payment/confirm-payment/${this.paymentId}`, null, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        }
      })
      .subscribe({
        next: (response) => {
          this.isProcessing = false;
          this.paymentSuccess = true;
          alert('Payment confirmed! Your booking is now confirmed.');
          this.fetchBookingDetails();
          
        },
        error: (err: HttpErrorResponse) => {
          this.isProcessing = false;
          console.error('Payment confirmation error:', err);
          alert('Failed to confirm payment. Error: ' + (err.error?.message || err.message));
        }
      });
  }

  fetchBookingDetails() {
    const token = this.authService.getToken();
    if (!token) return;

    this.isLoading = true;
    this.http
      .get<BookingResponseDto>(`http://localhost:5205/api/booking/${this.bookingId}`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        }
      })
      .subscribe({
        next: (response) => {
          this.bookingDetails = response;
          this.paymentSuccess = response.paymentStatus === 'Completed';
          this.isLoading = false;
          if (!this.paymentSuccess) {
            this.fetchPaymentId();
          }
        },
        error: (err: HttpErrorResponse) => {
          alert('Failed to fetch booking details. Error: ' + (err.error?.message || err.message));
          this.isLoading = false;
        }
      });
  }

  downloadTicket() {
    if (!this.bookingDetails) {
      alert('Booking details are not available.');
      return;
    }

    const totalAmount = this.bookingDetails.totalAmount;
    const baseFare = (totalAmount - 10) / 1.06;
    const gst = baseFare * 0.06;
    const convenienceFee = 10;

    const ticketContent = `
    BusBuddy Ticket
    ---------------
    Booking ID: ${this.bookingDetails.id}
    
    Journey Details:
    Source: ${this.bookingDetails.source}
    Destination: ${this.bookingDetails.destination}
    Journey Date: ${new Date(this.bookingDetails.journeyDate).toLocaleDateString()}
    
    Passenger Details:
    ${this.bookingDetails.ticketPassengers.map(p => `(Name: ${p.passengerName || 'N/A'}, Age: ${p.age || 'N/A'}, Gender: ${p.gender || 'N/A'}, Seat: ${p.seatNumber || 'N/A'}, Handicapped: ${p.isHandicapped ? 'Yes' : 'No'})`).join('\n')}
    
    Payment Details:
    Base Fare: ₹${baseFare.toFixed(2)}
    GST: ₹${gst.toFixed(2)}
    Convenience Fee: ₹${convenienceFee.toFixed(2)}
    Total Amount: ₹${totalAmount.toFixed(2)}
    Payment Method: ${this.bookingDetails.paymentMethod || 'Online'}
    Payment Status: ${this.bookingDetails.paymentStatus || 'Completed'}
    ---------------
    Thank you for choosing BusBuddy!
    `;

    const blob = new Blob([ticketContent], { type: 'text/plain;charset=utf-8' });
    saveAs(blob, `ticket_${this.bookingId}.txt`);
  }

  printTicket() {
    if (!this.bookingDetails) {
      alert('Booking details are not available.');
      return;
    }

    const totalAmount = this.bookingDetails.totalAmount;
    const baseFare = (totalAmount - 10) / 1.06;
    const gst = baseFare * 0.06;
    const convenienceFee = 10;

    const printContent = `
    <h2>BusBuddy Ticket</h2>
    <hr>
    <p><strong>Booking ID:</strong> ${this.bookingDetails.id}</p>
    <h3>Journey Details:</h3>
    <p><strong>Source:</strong> ${this.bookingDetails.source}</p>
    <p><strong>Destination:</strong> ${this.bookingDetails.destination}</p>
    <p><strong>Journey Date:</strong> ${new Date(this.bookingDetails.journeyDate).toLocaleDateString()}</p>
    <h3>Passenger Details:</h3>
    ${this.bookingDetails.ticketPassengers.map(p => `<p>(Name: ${p.passengerName || 'N/A'}, Age: ${p.age || 'N/A'}, Gender: ${p.gender || 'N/A'}, Seat: ${p.seatNumber || 'N/A'}, Handicapped: ${p.isHandicapped ? 'Yes' : 'No'})</p>`).join('')}
    <h3>Payment Details:</h3>
    <p><strong>Base Fare:</strong> ₹${baseFare.toFixed(2)}</p>
    <p><strong>GST:</strong> ₹${gst.toFixed(2)}</p>
    <p><strong>Convenience Fee:</strong> ₹${convenienceFee.toFixed(2)}</p>
    <p><strong>Total Amount:</strong> ₹${totalAmount.toFixed(2)}</p>
    <p><strong>Payment Method:</strong> ${this.bookingDetails.paymentMethod || 'Online'}</p>
    <p><strong>Payment Status:</strong> ${this.bookingDetails.paymentStatus || 'Completed'}</p>
    <hr>
    <p>Thank you for choosing BusBuddy!</p>
    `;

    const printWindow = window.open('', '_blank');
    if (printWindow) {
      printWindow.document.write(`
        <html>
          <head>
            <title>BusBuddy Ticket</title>
            <style>
              body { font-family: Arial, sans-serif; margin: 20px; }
              h2, h3 { color: #333; }
              p { margin: 5px 0; }
              hr { border: 0; border-top: 1px solid #ccc; margin: 10px 0; }
            </style>
          </head>
          <body>
            ${printContent}
          </body>
        </html>
      `);
      printWindow.document.close();
      printWindow.focus();
      printWindow.print();
    } else {
      alert('Please allow popups to print the ticket.');
    }
  }

  cancelPayment() {
    if (confirm('Are you sure you want to cancel the payment and return to the landing page?')) {
      alert('Payment cancelled. Returning to the landing page.');
      this.router.navigate(['/']);
    }
  }

  validatePayment(): boolean {
    if (!this.paymentMethod) {
      alert('Please select a payment method.');
      return false;
    }
    return true;
  }

  formatAmount(amount: number): string {
    return amount.toFixed(2);
  }

  returnToLandingPage() {
    this.router.navigate(['/']);
  }
}