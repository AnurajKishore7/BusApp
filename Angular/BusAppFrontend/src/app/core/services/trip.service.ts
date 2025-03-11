import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { RawTripResult, TripResults, parseTripResult, TripDetailsResponse } from '../models/trip-results.dto';

@Injectable({
  providedIn: 'root'
})
export class TripService {
  private apiUrl = 'http://localhost:5205/api/booking/search-trips';
  private tripDetailsUrl = 'http://localhost:5205/api/trips';

  constructor(private http: HttpClient) {}

  searchTrips(source: string, destination: string, journeyDate: string): Observable<TripResults[]> {
    let params = new HttpParams()
      .set('source', encodeURIComponent(source))
      .set('destination', encodeURIComponent(destination))
      .set('journeyDate', encodeURIComponent(journeyDate));

    return this.http.get<RawTripResult[]>(this.apiUrl, { params }).pipe(
      map((rawTrips: RawTripResult[]) => {
        return rawTrips.map(rawTrip => parseTripResult(rawTrip, source, destination));
      })
    );
  }

  getTripDetails(tripId: number, journeyDate: string): Observable<TripDetailsResponse> {
    const params = new HttpParams().set('journeyDate', journeyDate);
    return this.http.get<TripDetailsResponse>(`${this.tripDetailsUrl}/${tripId}/details`, { params });
  }
}