import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { RawTripResult, TripResults, parseTripResult } from '../models/trip-results.dto';

@Injectable({
  providedIn: 'root'
})
export class TripService {
  private apiUrl = 'http://localhost:5205/api/booking/search-trips';

  constructor(private http: HttpClient) {}

  searchTrips(source: string, destination: string, journeyDate: string, filters?: { priceMin?: number; priceMax?: number; busTypes?: string[] }): Observable<TripResults[]> {
    // Start with initial parameters
    let params = new HttpParams()
      .set('source', encodeURIComponent(source))
      .set('destination', encodeURIComponent(destination))
      .set('journeyDate', encodeURIComponent(journeyDate));

    // Add optional filters if they exist
    if (filters) {
      if (filters.priceMin) {
        params = params.set('priceMin', filters.priceMin.toString());
      }
      if (filters.priceMax) {
        params = params.set('priceMax', filters.priceMax.toString());
      }
      if (filters.busTypes && filters.busTypes.length) {
        params = params.set('busTypes', filters.busTypes.join(','));
      }
    }

    return this.http.get<RawTripResult[]>(this.apiUrl, { params }).pipe(
      map((rawTrips: RawTripResult[]) => {
        return rawTrips.map(rawTrip => parseTripResult(rawTrip));
      })
    );
  }
}