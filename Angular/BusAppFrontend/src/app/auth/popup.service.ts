import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PopupService {
  private openLoginPopupSubject = new Subject<void>();
  openLoginPopup$ = this.openLoginPopupSubject.asObservable();

  triggerLoginPopup() {
    this.openLoginPopupSubject.next();
  }
}