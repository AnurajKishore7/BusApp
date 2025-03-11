import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { TripService } from '../../core/services/trip.service';
import { TripResults, TimeSpan, TripDetailsResponse } from '../../core/models/trip-results.dto';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../auth/auth.service';
import { PopupService } from '../../auth/popup.service';
import { CapitalizePipe } from '../capitalize.pipe';

interface Seat {
  number: string;
  available: boolean;
  selected: boolean;
}

@Component({
  selector: 'app-trip-results',
  standalone: true,
  imports: [CommonModule, FormsModule, CapitalizePipe],
  templateUrl: './trip-results.component.html',
  styleUrls: ['./trip-results.component.css']
})
export class TripResultsComponent implements OnInit {
  source: string = '';
  destination: string = '';
  journeyDate: string = '';
  trips: TripResults[] = [];
  filteredTrips: TripResults[] = [];
  priceMin: number = 50;
  priceMax: number = 4799;
  selectedBusTypes: string[] = [];
  busTypeOptions: string[] = [
    'AC Seater',
    'AC Semi-Sleeper',
    'AC Sleeper',
    'Non-AC Seater',
    'Non-AC Semi-Sleeper',
    'Non-AC Sleeper'
  ];
  sortBy: string = 'fare';

  selectedTripId: number | null = null;
  seats: Seat[] = [];
  selectedSeats: string[] = [];
  isLoggedIn: boolean = false;
  isClient: boolean = false;
  loadingSeats: boolean = false;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private tripService: TripService,
    private authService: AuthService,
    private popupService: PopupService
  ) {}

  ngOnInit() {
    this.authService.isLoggedIn().subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
      this.authService.getRole().subscribe(role => {
        this.isClient = role === 'Client';
      });
    });

    this.route.queryParams.subscribe(params => {
      this.source = params['source'] || '';
      this.destination = params['destination'] || '';
      this.journeyDate = params['journeyDate'] || '';
      this.loadTrips();
    });
  }

  loading: boolean = false;

  loadTrips() {
    if (!this.source || !this.destination || !this.journeyDate) {
      this.error = 'Source, destination, and journey date are required to search for trips.';
      this.loading = false;
      return;
    }

    this.loading = true;
    this.tripService
      .searchTrips(this.source, this.destination, this.journeyDate)
      .subscribe({
        next: (trips: TripResults[]) => {
          this.trips = trips;
          this.applyFiltersAndSort();
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load trips. Please try again later.';
          this.loading = false;
          console.error('API Error:', err);
        }
      });
  }

  applyFiltersAndSort() {
    const now = new Date();
    const currentDate = new Date(now.getTime() - now.getTimezoneOffset() * 60000).toISOString().split('T')[0];
    const currentTime = now.getHours() * 60 + now.getMinutes();

    this.filteredTrips = [...this.trips]
      .filter(trip => {
        const tripDate = this.journeyDate;
        const departureTime = trip.departure.hours * 60 + trip.departure.minutes;
        if (tripDate === currentDate) {
          return departureTime > currentTime;
        }
        return true;
      })
      .filter(trip => trip.startingFare >= this.priceMin && trip.startingFare <= this.priceMax)
      .filter(trip => this.selectedBusTypes.length === 0 || this.selectedBusTypes.includes(trip.busType))
      .sort((a, b) => {
        if (this.sortBy === 'fare') return a.startingFare - b.startingFare;
        if (this.sortBy === 'seatsAvailable') return b.seatsAvailable - a.seatsAvailable;
        return 0;
      });
  }

  calculateDuration(departure: TimeSpan, arrival: TimeSpan): string {
    let depMinutes = departure.hours * 60 + departure.minutes;
    let arrMinutes = arrival.hours * 60 + arrival.minutes;
    if (arrMinutes < depMinutes) arrMinutes += 24 * 60;
    let diffMinutes = arrMinutes - depMinutes;
    const hours = Math.floor(diffMinutes / 60);
    const minutes = diffMinutes % 60;
    return `${hours}h ${minutes}m`;
  }

  onPriceChange() {
    this.applyFiltersAndSort();
  }

  onBusTypeChange(type: string, event: Event) {
    const checked = (event.target as HTMLInputElement).checked;
    if (checked) {
      this.selectedBusTypes.push(type);
    } else {
      this.selectedBusTypes = this.selectedBusTypes.filter(t => t !== type);
    }
    this.applyFiltersAndSort();
  }

  onSortChange(sortOption: string) {
    this.sortBy = sortOption;
    this.applyFiltersAndSort();
  }

  modifySearch() {
    this.router.navigate([''], {
      queryParams: {
        source: this.source,
        destination: this.destination,
        journeyDate: this.journeyDate
      }
    });
  }

  viewSeats(tripId: number) {
    if (this.selectedTripId === tripId) {
      this.selectedTripId = null;
      this.seats = [];
      this.selectedSeats = [];
      return;
    }

    this.selectedTripId = tripId;
    this.selectedSeats = [];
    this.loadingSeats = true;
    this.error = null;

    this.tripService.getTripDetails(tripId, this.journeyDate).subscribe({
      next: (tripDetails: TripDetailsResponse) => {
        this.initializeSeats(tripDetails);
        this.loadingSeats = false;
      },
      error: (err) => {
        this.error = 'Failed to load seat details. Please try again later.';
        this.loadingSeats = false;
      }
    });
  }

  initializeSeats(tripDetails: TripDetailsResponse) {
    this.seats = [];
    for (let i = 1; i <= 20; i++) {
      this.seats.push({ number: `A${i}`, available: false, selected: false });
      this.seats.push({ number: `B${i}`, available: false, selected: false });
    }

    const availableSeatNumbers = new Set(tripDetails.availableSeats);
    this.seats.forEach(seat => {
      seat.available = availableSeatNumbers.has(seat.number);
    });
  }

  toggleSeatSelection(seat: Seat) {
    if (!seat.available) return;

    seat.selected = !seat.selected;
    if (seat.selected) {
      this.selectedSeats.push(seat.number);
    } else {
      this.selectedSeats = this.selectedSeats.filter(s => s !== seat.number);
    }
  }

  proceedToBooking(tripId: number) {
    if (this.selectedSeats.length === 0) {
      this.error = 'Please select at least one seat to proceed.';
      return;
    }

    if (!this.isLoggedIn || !this.isClient) {
      this.popupService.triggerLoginPopup();
      return;
    }

    const trip = this.trips.find(t => t.tripId === tripId);
    if (!trip) {
      this.error = 'Trip not found. Please try again.';
      return;
    }

    const serializableTrip = {
      tripId: trip.tripId,
      operatorName: trip.operatorName,
      busNo: trip.busNo,
      departure: trip.departure.toString(),
      arrival: trip.arrival.toString(),
      startingFare: trip.startingFare,
      seatsAvailable: trip.seatsAvailable,
      totalSeats: trip.totalSeats,
      busType: trip.busType,
      departureLocation: this.source,
      arrivalLocation: this.destination,
      availableSeats: trip.availableSeats
    };

    this.router.navigate(['/booking'], {
      queryParams: {
        trip: JSON.stringify(serializableTrip),
        selectedSeats: JSON.stringify(this.selectedSeats),
        journeyDate: this.journeyDate,
        source: this.source,
        destination: this.destination
      }
    });
  }
}