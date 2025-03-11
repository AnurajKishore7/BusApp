import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PopupService {
  private openLoginPopupSubject = new Subject<void>();
  openLoginPopup$ = this.openLoginPopupSubject.asObservable();

  private loginSuccessSubject = new Subject<void>();
  loginSuccess$ = this.loginSuccessSubject.asObservable();

  triggerLoginPopup() {
    this.openLoginPopupSubject.next();
  }

  notifyLoginSuccess() { 
    this.loginSuccessSubject.next();
  }
}