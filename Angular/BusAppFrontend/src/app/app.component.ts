import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent } from '@ng-icons/core';
import { NavbarComponent } from './navbar/navbar.component';
import { SignupClientComponent } from './auth/signup-client/signup-client.component';
import { SignupOperatorComponent } from './auth/signup-operator/signup-operator.component';
import { LandingComponent } from './auth/landing/landing.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    NgIconComponent,
    NavbarComponent,
    SignupClientComponent,
    SignupOperatorComponent,
    LandingComponent,
    RouterOutlet
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'BusBuddy';
}