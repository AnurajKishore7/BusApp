import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-trip-results',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './trip-results.component.html',
  styleUrls: ['./trip-results.component.css']
})
export class TripResultsComponent implements OnInit {
  source: string = '';
  destination: string = '';
  journeyDate: string = '';

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.source = params['source'] || '';
      this.destination = params['destination'] || '';
      this.journeyDate = params['journeyDate'] || '';
    });
  }
}