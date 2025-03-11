import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Bus } from '../../core/models/bus.dto';
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
  searchBusNo: string = '';
  searchType: string = '';
  searchOperatorId: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;
  errorMessage: string | null = null;
  busTypes: string[] = [
    'AC Sleeper',
    'AC Seater',
    'Non-AC Sleeper',
    'Non-AC Seater',
    'AC Semi-Sleeper',
    'Non-AC Semi-Sleeper'
  ];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.fetchBuses();
  }

  fetchBuses() {
    this.http.get<Bus[]>(`${environment.apiUrl}/buses/all`).subscribe({
      next: (buses) => {
        this.buses = buses;
        this.applyFiltersAndPagination();
      },
      error: (err) => {
        this.errorMessage = 'Failed to load buses: ' + (err.statusText || 'Unknown error');
      }
    });
  }

  applyFiltersAndPagination() {
    let filtered = this.buses;
    if (this.searchBusNo) {
      filtered = filtered.filter(bus => bus.busNo.toLowerCase().includes(this.searchBusNo.toLowerCase()));
    }
    if (this.searchType) { 
      filtered = filtered.filter(bus => bus.busType.toLowerCase() === this.searchType.toLowerCase());
    }
    if (this.searchOperatorId) {
      filtered = filtered.filter(bus => bus.operatorId.toString().includes(this.searchOperatorId));
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
}