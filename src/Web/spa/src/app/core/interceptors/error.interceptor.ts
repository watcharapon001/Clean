import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const snackBar = inject(MatSnackBar);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unknown error occurred!';
      
      if (error.error instanceof ErrorEvent) {
        // Client-side or network error
        errorMessage = `Network Error: ${error.error.message}`;
      } else {
        // Backend returns ProblemDetails JSON
        if (error.error && error.error.title) {
          errorMessage = error.error.title;
          if (error.error.detail) {
            errorMessage += `: ${error.error.detail}`;
          }
          
          // If there are validation errors, append them
          if (error.error.errors) {
            const validationErrors = error.error.errors;
            const errorKeys = Object.keys(validationErrors);
            if (errorKeys.length > 0) {
              const firstKey = errorKeys[0];
              const firstError = Array.isArray(validationErrors[firstKey]) 
                ? validationErrors[firstKey][0] 
                : validationErrors[firstKey];
              errorMessage += ` - ${firstError}`;
            }
          }
        } else if (error.status === 0) {
          errorMessage = 'Server is unreachable. Please check your connection.';
        } else {
          errorMessage = `HTTP Error ${error.status}: ${error.statusText}`;
        }
      }

      // Automatically show the error message
      snackBar.open(errorMessage, 'Close', {
        duration: 5000,
        horizontalPosition: 'end',
        verticalPosition: 'bottom',
        panelClass: ['bg-danger', 'text-white']
      });

      return throwError(() => error);
    })
  );
};
