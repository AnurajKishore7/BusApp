import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { PopupService } from '../popup.service';
import { ClientRegister } from '../../core/models/client-register.dto';
import { NgIconComponent } from '@ng-icons/core';

@Component({
  selector: 'app-signup-client',
  standalone: true,
  imports: [CommonModule, FormsModule, NgIconComponent],
  templateUrl: './signup-client.component.html',
  styleUrls: ['./signup-client.component.css']
})
export class SignupClientComponent {
  client: ClientRegister = {
    name: '',
    email: '',
    password: '',
    dob: '',
    gender: '',
    contact: '',
    isHandicapped: false
  };
  errorMessage: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private popupService: PopupService
  ) {}

  onSubmit() {
    this.authService.registerClient(this.client).subscribe({
      next: () => {
        this.router.navigate(['/trip-search']);
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