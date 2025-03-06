import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Trip } from '../../core/models/trip.dto';
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
  errorMessage: string | null = null;
  searchBusRouteId: string = '';
  currentPage: number = 1;
  pageSize: number = 8;
  totalPages: number = 1;

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
}