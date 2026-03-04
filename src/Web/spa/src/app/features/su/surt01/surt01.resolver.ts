import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { Surt01Service } from './surt01.service';
import { Profile } from './surt01.component';

export const surt01Resolver: ResolveFn<Profile | null> = (route, state) => {
  const id = route.paramMap.get('id');
  const service = inject(Surt01Service);
  const router = inject(Router);

  if (!id || id === 'new') {
    return null;
  }

  return service.getProfile(id).pipe(
    catchError(() => {
      router.navigate(['/su/surt01']);
      return EMPTY;
    })
  );
};
