import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const token = localStorage.getItem('authToken');

  if (token) {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const isExpired = payload.exp * 1000 < Date.now(); 

      if (isExpired) {
        console.warn('Token expired. Redirecting to login.');
        localStorage.removeItem('authToken');
        router.navigate(['/login']);
        return next(req);
      }
    } catch (error) {
      console.error('Error decoding token:', error);
      localStorage.removeItem('authToken');
      router.navigate(['/login']);
      return next(req);
    }
  }

  const authReq = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      })
    : req;

  return next(authReq);
};