import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BusRoute, NewBusRoute, UpdateBusRoute } from '../../core/models/bus-route.dto';
import { environment } from '../../../environments/environment.development';

@Component({
  selector: 'app-bus-routes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './bus-routes.component.html',
  styleUrls: ['./bus-routes.component.css']
})
export class BusRoutesComponent implements OnInit {
  busRoutes: BusRoute[] = [];
  filteredBusRoutes: BusRoute[] = [];
  successMessage: string | null = null;
  errorMessage: string | null = null;
  searchSource: string = '';
  searchDestination: string = '';
  currentPage: number = 1;
  pageSize: number = 7;
  totalPages: number = 1;
  showAddModal: boolean = false;
  showEditModal: boolean = false;
  newBusRoute: NewBusRoute = { source: null, destination: null, estimatedDuration: '', distance: 0 };
  editBusRoute: UpdateBusRoute = { source: null, destination: null, estimatedDuration: '', distance: 0 };
  editBusRouteId: number | null = null;
  isSubmitting: boolean = false;

  constructor(private http: HttpClient, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchBusRoutes();
  }

  fetchBusRoutes() {
    this.http.get<BusRoute[]>(`${environment.apiUrl}/busRoutes`).subscribe({
      next: (routes) => {
        this.busRoutes = routes;
        this.applyFiltersAndPagination();
      },
      error: (err) => {
        this.errorMessage = err.error || 'Failed to load bus routes';
      }
    });
  }

  addBusRoute() {
    if (this.isSubmitting) return;
    this.isSubmitting = true;
    this.cdr.detectChanges();
    console.log('Sending new bus route:', this.newBusRoute); // Debug
    this.http.post<BusRoute>(`${environment.apiUrl}/busRoutes`, this.newBusRoute).subscribe({
      next: (response) => {
        this.successMessage = 'Bus route added successfully.';
        this.fetchBusRoutes();
        this.showAddModal = false;
        this.newBusRoute = { source: null, destination: null, estimatedDuration: '', distance: 0 };
        this.isSubmitting = false;
        this.cdr.detectChanges();
        setTimeout(() => this.successMessage = null, 3000);
      },
      error: (err) => {
        console.error('Error response:', err); // Debug
        this.errorMessage = err.error || 'Failed to add bus route';
        this.isSubmitting = false;
        this.cdr.detectChanges();
        setTimeout(() => this.errorMessage = null, 3000);
      }
    });
  }

  updateBusRoute() {
    if (this.editBusRouteId === null) return;
    this.http.put(`${environment.apiUrl}/busRoutes/${this.editBusRouteId}`, this.editBusRoute, { responseType: 'text' }).subscribe({
      next: () => {
        this.successMessage = 'Bus route updated successfully.';
        this.fetchBusRoutes();
        this.showEditModal = false;
        this.editBusRouteId = null;
        this.editBusRoute = { source: null, destination: null, estimatedDuration: '', distance: 0 };
        this.cdr.detectChanges();
        setTimeout(() => this.successMessage = null, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error || 'Failed to update bus route';
        this.cdr.detectChanges();
        setTimeout(() => this.errorMessage = null, 3000);
      }
    });
  }

  deleteBusRoute(id: number) {
    this.http.delete(`${environment.apiUrl}/busRoutes/${id}`, { responseType: 'text' }).subscribe({
      next: () => {
        this.successMessage = 'Bus route deleted successfully.';
        this.fetchBusRoutes();
        this.cdr.detectChanges();
        setTimeout(() => this.successMessage = null, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error || 'Failed to delete bus route';
        this.cdr.detectChanges();
        setTimeout(() => this.errorMessage = null, 3000);
      }
    });
  }

  applyFiltersAndPagination() {
    let filtered = this.busRoutes;
    if (this.searchSource) {
      filtered = filtered.filter(route =>
        route.source.toLowerCase().includes(this.searchSource.toLowerCase())
      );
    }
    if (this.searchDestination) {
      filtered = filtered.filter(route =>
        route.destination.toLowerCase().includes(this.searchDestination.toLowerCase())
      );
    }
    this.totalPages = Math.ceil(filtered.length / this.pageSize);
    const start = (this.currentPage - 1) * this.pageSize;
    this.filteredBusRoutes = filtered.slice(start, start + this.pageSize);
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
    this.newBusRoute = { source: null, destination: null, estimatedDuration: '', distance: 0 };
  }

  openEditModal(route: BusRoute) {
    this.editBusRouteId = route.id;
    this.editBusRoute = {
      source: route.source,
      destination: route.destination,
      estimatedDuration: route.estimatedDuration.slice(0, 5), // Convert "HH:mm:ss" to "HH:mm"
      distance: route.distance
    };
    this.showEditModal = true;
  }

  closeEditModal() {
    this.showEditModal = false;
    this.editBusRouteId = null;
    this.editBusRoute = { source: null, destination: null, estimatedDuration: '', distance: 0 };
  }
}