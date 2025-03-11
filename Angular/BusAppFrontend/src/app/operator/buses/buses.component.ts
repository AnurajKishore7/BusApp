import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Bus } from '../../core/models/bus.dto';
import { NewBus, UpdateBus } from '../../core/models/bus-dtos';
import { environment } from '../../../environments/environment.development';

@Component({
  selector: 'app-buses',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './buses.component.html',
  styleUrls: ['./buses.component.css']
})
export class BusesComponent implements OnInit {
  buses: Bus[] = [];
  filteredBuses: Bus[] = [];
  successMessage: string | null = null;
  errorMessage: string | null = null;
  searchBusNo: string = '';
  searchType: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;
  showAddModal: boolean = false;
  showEditModal: boolean = false;
  newBus: NewBus = { busNo: null, busType: '', totalSeats: 0 };
  editBus: UpdateBus = { busNo: null, busType: null, totalSeats: 0 };
  editBusId: number | null = null;
  isSubmitting: boolean = false;
  busTypes: string[] = [
    'AC Sleeper',
    'AC Seater',
    'Non-AC Sleeper',
    'Non-AC Seater',
    'AC Semi-Sleeper',
    'Non-AC Semi-Sleeper'
  ];

  constructor(private http: HttpClient, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.fetchBuses();
  }

  fetchBuses() {
    this.http.get<Bus[]>(`${environment.apiUrl}/buses/my-buses`).subscribe({
      next: (buses) => {
        this.buses = buses;
        this.applyFiltersAndPagination();
      },
      error: (err) => {
        this.errorMessage = 'Failed to load buses: ' + (err.statusText || 'Unknown error');
      }
    });
  }

  addBus() {
    if (this.isSubmitting) return;
    this.isSubmitting = true;
    this.cdr.detectChanges();
    this.http.post(`${environment.apiUrl}/buses`, this.newBus, { responseType: 'text' }).subscribe({
      next: (response) => {
        this.successMessage = response || 'Bus added successfully.';
        this.fetchBuses();
        this.showAddModal = false;
        this.newBus = { busNo: null, busType: '', totalSeats: 0 };
        this.isSubmitting = false;
        this.cdr.detectChanges();
        setTimeout(() => this.successMessage = null, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error || 'Failed to add bus';
        this.isSubmitting = false;
        this.cdr.detectChanges();
        setTimeout(() => this.errorMessage = null, 3000);
      }
    });
  }

  updateBus() {
    if (this.editBusId === null) return;
    this.http.put(`${environment.apiUrl}/buses/${this.editBusId}`, this.editBus, { responseType: 'text' }).subscribe({
      next: (response) => {
        this.successMessage = response || 'Bus updated successfully.';
        this.fetchBuses(); // Refresh the list from API
        this.showEditModal = false;
        this.editBusId = null;
        this.editBus = { busNo: null, busType: null, totalSeats: 0 };
        this.cdr.detectChanges();
        setTimeout(() => this.successMessage = null, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error || 'Failed to update bus';
        this.cdr.detectChanges();
        setTimeout(() => this.errorMessage = null, 3000);
      }
    });
  }

  deleteBus(id: number) {
    this.http.delete(`${environment.apiUrl}/buses/${id}`, { responseType: 'text' }).subscribe({
      next: (response) => {
        this.successMessage = response || 'Bus deleted successfully.';
        this.fetchBuses();
        this.cdr.detectChanges();
        setTimeout(() => this.successMessage = null, 3000);
      },
      error: (err) => {
        this.errorMessage = err.error || 'Failed to delete bus';
        this.cdr.detectChanges();
        setTimeout(() => this.errorMessage = null, 3000);
      }
    });
  }

  applyFiltersAndPagination() {
    let filtered = this.buses;
    if (this.searchBusNo) {
      filtered = filtered.filter(bus => bus.busNo?.toLowerCase().includes(this.searchBusNo.toLowerCase()));
    }
    if (this.searchType) {
      filtered = filtered.filter(bus => bus.busType?.toLowerCase() === this.searchType.toLowerCase());
    }
    this.totalPages = Math.ceil(filtered.length / this.pageSize);
    const start = (this.currentPage - 1) * this.pageSize;
    this.filteredBuses = filtered.slice(start, start + this.pageSize);
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
    this.newBus = { busNo: null, busType: '', totalSeats: 0 };
  }

  openEditModal(bus: Bus) {
    this.editBusId = bus.id;
    this.editBus = { busNo: bus.busNo, busType: bus.busType, totalSeats: bus.totalSeats };
    this.showEditModal = true;
  }

  closeEditModal() {
    this.showEditModal = false;
    this.editBusId = null;
    this.editBus = { busNo: null, busType: null, totalSeats: 0 };
  }
}