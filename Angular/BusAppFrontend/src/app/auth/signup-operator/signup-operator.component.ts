import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { PopupService } from '../popup.service';
import { TransportRegister } from '../../core/models/transport-register.dto';
import { NgIconComponent } from '@ng-icons/core';

@Component({
  selector: 'app-signup-operator',
  standalone: true,
  imports: [CommonModule, FormsModule, NgIconComponent],
  templateUrl: './signup-operator.component.html',
  styleUrls: ['./signup-operator.component.css']
})
export class SignupOperatorComponent {
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
    private popupService: PopupService
  ) {}

  onSubmit() {
    this.authService.registerOperator(this.operator).subscribe({
      next: (message) => {
        this.successMessage = message;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Registration failed';
      }
    });
  }

  openLoginPopup() {
    this.popupService.triggerLoginPopup();
  }

  closeSignup() {
    this.router.navigate(['/']);
  }
}