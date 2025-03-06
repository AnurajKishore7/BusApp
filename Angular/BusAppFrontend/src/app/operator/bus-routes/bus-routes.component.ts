import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BusRoute } from '../../core/models/bus-route.dto';
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
  errorMessage: string | null = null;
  searchSource: string = '';
  searchDestination: string = '';
  currentPage: number = 1;
  pageSize: number = 7;
  totalPages: number = 1;

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
}