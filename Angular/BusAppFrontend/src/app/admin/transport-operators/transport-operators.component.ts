import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../auth/auth.service';
import { FormsModule } from '@angular/forms'; // For ngModel
import { PendingOperator } from '../../core/models/pending-operator.dto';
import { ApproveResponse } from '../../core/models/approve-response.dto';
import { environment } from '../../../environments/environment.development';

@Component({
  selector: 'app-transport-operators',
  standalone: true,
  imports: [CommonModule, FormsModule], // Add FormsModule
  templateUrl: './transport-operators.component.html',
  styleUrls: ['./transport-operators.component.css']
})
export class TransportOperatorsComponent implements OnInit {
  operators: PendingOperator[] = [];
  filteredOperators: PendingOperator[] = [];
  successMessage: string | null = null;
  errorMessage: string | null = null;
  searchName: string = '';
  searchEmail: string = '';
  currentPage: number = 1;
  pageSize: number = 5; // Rows per page
  totalPages: number = 1;

  constructor(private http: HttpClient, private authService: AuthService) {}

  ngOnInit() {
    this.fetchPendingOperators();
  }

  fetchPendingOperators() {
    this.http.get<PendingOperator[]>(`${environment.apiUrl}/auth/pending-operators`).subscribe({
      next: (operators) => {
        this.operators = operators;
        this.applyFiltersAndPagination();
      },
      error: (err) => {
        this.errorMessage = 'Failed to load pending operators: ' + (err.statusText || 'Unknown error');
      }
    });
  }

  approveOperator(name: string) {
    this.http.put<ApproveResponse>(`${environment.apiUrl}/auth/approve-operator/${name}`, {}).subscribe({
      next: (response) => {
        this.successMessage = response.message;
        this.operators = this.operators.filter(op => op.name !== name);
        this.applyFiltersAndPagination();
        setTimeout(() => this.successMessage = null, 3000);
      },
      error: () => {
        this.errorMessage = 'Approval failed';
        setTimeout(() => this.errorMessage = null, 3000);
      }
    });
  }

  applyFiltersAndPagination() {
    let filtered = this.operators;
    if (this.searchName) {
      filtered = filtered.filter(op => op.name.toLowerCase().includes(this.searchName.toLowerCase()));
    }
    if (this.searchEmail) {
      filtered = filtered.filter(op => op.email.toLowerCase().includes(this.searchEmail.toLowerCase()));
    }
    this.totalPages = Math.ceil(filtered.length / this.pageSize);
    const start = (this.currentPage - 1) * this.pageSize;
    this.filteredOperators = filtered.slice(start, start + this.pageSize);
  }

  onSearchChange() {
    this.currentPage = 1; // Reset to first page on search
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