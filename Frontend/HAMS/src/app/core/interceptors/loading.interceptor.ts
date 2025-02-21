import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs';
import { LoadingService } from '../../services/loading.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService); // Inject LoadingService dynamically
  loadingService.showLoading(); // Show loading spinner

  return next(req).pipe(
    finalize(() => loadingService.hideLoading()) // Hide spinner when request completes
  );
};