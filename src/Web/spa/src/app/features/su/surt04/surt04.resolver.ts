import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { catchError, map } from 'rxjs/operators';
import { forkJoin, of, EMPTY } from 'rxjs';
import { Surt04Service, User, Employee, Organization } from './surt04.service';
import { Surt01Service } from '../surt01/surt01.service';
import { Profile } from '../surt01/surt01.component';

export interface Surt04DetailData {
  user: User | null;
  employees: Employee[];
  profiles: Profile[];
  organizations: Organization[];
}

export const surt04Resolver: ResolveFn<Surt04DetailData> = (route, state) => {
  const id = route.paramMap.get('id');
  const service = inject(Surt04Service);
  const profileService = inject(Surt01Service);
  const router = inject(Router);

  const employees$ = service.getEmployees(1, 1000).pipe(map(res => res.items));
  const profiles$ = profileService.getProfiles(1, 1000).pipe(map(res => res.items));
  const orgs$ = service.getOrganizes();
  const user$ = (id && id !== 'new') ? service.getUser(id) : of(null);

  return forkJoin({
    user: user$,
    employees: employees$,
    profiles: profiles$,
    organizations: orgs$
  }).pipe(
    catchError(() => {
      router.navigate(['/su/surt04']);
      return EMPTY;
    })
  );
};
