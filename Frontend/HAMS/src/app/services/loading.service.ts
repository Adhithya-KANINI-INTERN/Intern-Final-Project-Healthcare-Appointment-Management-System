import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private loadingSubject = new BehaviorSubject<boolean>(false);
  loading$ = this.loadingSubject.asObservable();

  showLoading(): void {
    this.loadingSubject.next(true); // Trigger loading state
  }

  hideLoading(): void {
    this.loadingSubject.next(false); // End loading state
  }
}
