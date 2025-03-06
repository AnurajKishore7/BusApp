import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { heroTruck, heroMapPin, heroCalendar, heroArrowsUpDown } from '@ng-icons/heroicons/outline';
import { ionLogoFacebook, ionLogoInstagram, ionLogoTwitter, ionLogoLinkedin } from '@ng-icons/ionicons';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [CommonModule, FormsModule, NgIconComponent],
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css'],
  providers: [
    provideIcons({
      heroTruck,
      heroMapPin,
      heroCalendar,
      heroArrowsUpDown,
      ionLogoFacebook,
      ionLogoInstagram,
      ionLogoTwitter,
      ionLogoLinkedin
    })
  ]
})
export class LandingComponent implements OnInit, AfterViewInit {
  fromStation: string = '';
  toStation: string = '';
  journeyDate: string = '';
  minDate: string = '';
  navbarHeight: number = 0; 
  message: string = '';

  constructor(private router: Router) {}

  ngOnInit() {
    const now = new Date();
    const localDate = new Date(now.getTime() - now.getTimezoneOffset() * 60000);
    this.journeyDate = localDate.toISOString().split('T')[0]; 
    this.minDate = this.journeyDate; 
    this.updateNavbarHeight();
  }

  ngAfterViewInit() {
    this.updateNavbarHeight();
  }

  private updateNavbarHeight() {
    const navbar = document.querySelector('nav'); 
    if (navbar) {
      this.navbarHeight = navbar.getBoundingClientRect().height; 
    }
  }

  setToday() {
    const now = new Date();
    const localDate = new Date(now.getTime() - now.getTimezoneOffset() * 60000);
    this.journeyDate = localDate.toISOString().split('T')[0]; // "2025-03-07"
  }

  setTomorrow() {
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    const localTomorrow = new Date(tomorrow.getTime() - tomorrow.getTimezoneOffset() * 60000);
    this.journeyDate = localTomorrow.toISOString().split('T')[0]; // "2025-03-08"
  }

  swapStations() {
    [this.fromStation, this.toStation] = [this.toStation, this.fromStation];
  }

  onSearch() {
    if (!this.fromStation || !this.toStation) {
      this.message = 'Please enter both From and To stations.'; 
      return;
    }
    this.message = ''; 
    this.router.navigate(['/trip-results'], {
      queryParams: {
        source: this.fromStation,
        destination: this.toStation,
        journeyDate: this.journeyDate
      }
    });
  }
}