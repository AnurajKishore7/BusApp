import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { TripService } from '../../core/services/trip.service';
import { TripResults, TimeSpan } from '../../core/models/trip-results.dto';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-trip-results',
  standalone: true,
  imports: [CommonModule, FormsModule],
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

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private tripService: TripService
  ) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.source = params['source'] || '';
      this.destination = params['destination'] || '';
      this.journeyDate = params['journeyDate'] || '';
      this.loadTrips();
    });
  }

  loading: boolean = false;
  error: string | null = null;

  loadTrips() {
    this.tripService
      .searchTrips(this.source, this.destination, this.journeyDate, {
        priceMin: this.priceMin,
        priceMax: this.priceMax,
        busTypes: this.selectedBusTypes
      })
      .subscribe({
        next: (trips) => {
          this.trips = trips;
          this.applyFiltersAndSort();
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load trips. Please try again later.';
          this.loading = false;
        }
      });
  }

  applyFiltersAndSort() {
    const now = new Date(); // Current time: March 6, 2025, 10:00 AM (IST)
    const currentDate = new Date(now.getTime() - now.getTimezoneOffset() * 60000).toISOString().split('T')[0];
    const currentTime = now.getHours() * 60 + now.getMinutes(); // 10:00 AM = 600 minutes

    this.filteredTrips = [...this.trips]
      .filter(trip => {
        const tripDate = this.journeyDate;
        const departureTime = trip.departure.hours * 60 + trip.departure.minutes;
        if (tripDate === currentDate) {
          return departureTime > currentTime; // Filter out past trips today
        }
        return true; // Include all trips for future dates
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
    if (arrMinutes < depMinutes) arrMinutes += 24 * 60; // Handle overnight trips
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
    this.router.navigate([`/trip-details/${tripId}`]);
  }
}