import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { Dbrt01Service, Dbrt01 } from './dbrt01.service';

export const dbrt01Resolver: ResolveFn<Dbrt01 | null> = (route, state) => {
  const id = route.paramMap.get('id');
  const service = inject(Dbrt01Service);
  const router = inject(Router);

  if (!id || id === 'new') {
    return null;
  }

  return service.getDbrt01(id).pipe(
    catchError(() => {
      router.navigate(['/db/dbrt01']);
      return EMPTY;
    })
  );
};
