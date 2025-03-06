import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Trip, NewTrip, UpdateTrip } from '../../core/models/trip.dto';
import { environment } from '../../../environments/environment.development';

@Component({
  selector: 'app-trips',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trips.component.html',
  styleUrls: ['./trips.component.css']
})
export class TripsComponent implements OnInit {
  trips: Trip[] = [];
  filteredTrips: Trip[] = [];
  successMessage: string | null = null;
  errorMessage: string | null = null;
  searchBusRouteId: string = '';
  searchSource: string = '';
  searchDestination: string = '';
  currentPage: number = 1;
  pageSize: number = 8;
  totalPages: number = 1;
  showAddModal: boolean = false;
  showEditModal: boolean = false;
  newTrip: NewTrip = { busRouteId: 0, busId: 0, departureTime: '', arrivalTime: '', price: 0 };
  editTrip: UpdateTrip = { busRouteId: 0, busId: 0, departureTime: '', arrivalTime: '', price: 0 };
  editTripId: number | null = null;
  isSubmitting: boolean = false;

  constructor(private http: HttpClient, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchTrips();
  }

  fetchTrips() {
    this.http.get<Trip[]>(`${environment.apiUrl}/trips`).subscribe({
      next: (trips) => {
        this.trips = trips;
        this.applyFiltersAndPagination();
      },
      error: (err) => {
        this.errorMessage = err.error || 'Failed to load trips';
      }
    });
  }

  addTrip() {
    if (this.isSubmitting) return;
    this.isSubmitting = true;
    this.cdr.detectChanges();
    this.http.post<Trip>(`${environment.apiUrl}/trips`, this.newTrip).subscribe({
      next: (response) => {
        this.successMessage = 'Trip added successfully.';
        this.fetchTrips();
        this.showAddModal = false;
        this.newTrip = { busRouteId: 0, busId: 0, departureTime: '', arrivalTime: '', price: 0 };
        this.isSubmitting = false;
        this.cdr.detectChanges();
        setTimeout(() => this.successMessage = null, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error || 'Failed to add trip';
        this.isSubmitting = false;
        this.cdr.detectChanges();
        setTimeout(() => this.errorMessage = null, 3000);
      }
    });
  }

  updateTrip() {
    if (this.editTripId === null) return;
    this.http.put<Trip>(`${environment.apiUrl}/trips/${this.editTripId}`, this.editTrip).subscribe({
      next: (response) => {
        this.successMessage = 'Trip updated successfully.';
        this.fetchTrips();
        this.showEditModal = false;
        this.editTripId = null;
        this.editTrip = { busRouteId: 0, busId: 0, departureTime: '', arrivalTime: '', price: 0 };
        this.cdr.detectChanges();
        setTimeout(() => this.successMessage = null, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error || 'Failed to update trip';
        this.cdr.detectChanges();
        setTimeout(() => this.errorMessage = null, 3000);
      }
    });
  }

  deleteTrip(id: number) {
    this.http.delete(`${environment.apiUrl}/trips/${id}`, { responseType: 'text' }).subscribe({
      next: () => {
        this.successMessage = 'Trip deleted successfully.';
        this.fetchTrips();
        this.cdr.detectChanges();
        setTimeout(() => this.successMessage = null, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error || 'Failed to delete trip';
        this.cdr.detectChanges();
        setTimeout(() => this.errorMessage = null, 3000);
      }
    });
  }

  applyFiltersAndPagination() {
    let filtered = this.trips;
    if (this.searchBusRouteId) {
      filtered = filtered.filter(trip => 
        trip.busRouteId.toString().includes(this.searchBusRouteId)
      );
    }
    this.totalPages = Math.ceil(filtered.length / this.pageSize);
    const start = (this.currentPage - 1) * this.pageSize;
    this.filteredTrips = filtered.slice(start, start + this.pageSize);
  }

  onSearchChange() {
    this.currentPage = 1;
    this.applyFiltersAndPagination();
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.applyFiltersAndPagination();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.applyFiltersAndPagination();
    }
  }

  openAddModal() {
    this.showAddModal = true;
  }

  closeAddModal() {
    this.showAddModal = false;
    this.newTrip = { busRouteId: 0, busId: 0, departureTime: '', arrivalTime: '', price: 0 };
  }

  openEditModal(trip: Trip) {
    this.editTripId = trip.id;
    this.editTrip = {
      busRouteId: trip.busRouteId,
      busId: trip.busId,
      departureTime: trip.departureTime.slice(0, 5), // "HH:mm:ss" to "HH:mm"
      arrivalTime: trip.arrivalTime.slice(0, 5),     // "HH:mm:ss" to "HH:mm"
      price: trip.price
    };
    this.showEditModal = true;
  }

  closeEditModal() {
    this.showEditModal = false;
    this.editTripId = null;
    this.editTrip = { busRouteId: 0, busId: 0, departureTime: '', arrivalTime: '', price: 0 };
  }
}