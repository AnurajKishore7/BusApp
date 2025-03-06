import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { PopupService } from '../popup.service';// Add this
import { TransportRegister } from '../../core/models/transport-register.dto';
import { NgIconComponent } from '@ng-icons/core';

@Component({
  selector: 'app-signup-operator',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './landing.component.html', // Fix this
  styleUrls: ['./landing.component.css']
})
export class LandingComponent {
  operator: TransportRegister = {
    name: '',
    email: '',
    password: ''
  };
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private popupService: PopupService // Add this
  ) {}

  onSubmit() {
    this.authService.registerOperator(this.operator).subscribe({
      next: (message) => {
        this.successMessage = message;
        setTimeout(() => this.popupService.triggerLoginPopup(), 2000); // Update this
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Registration failed';
      }
    });
  }

  openLoginPopup() {
    this.popupService.triggerLoginPopup(); // Use service
  }

  closeSignup() {
    this.router.navigate(['/']);
  }
}