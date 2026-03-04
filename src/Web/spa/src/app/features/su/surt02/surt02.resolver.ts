import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { catchError, map } from 'rxjs/operators';
import { forkJoin, of, EMPTY } from 'rxjs';
import { Surt02Service } from './surt02.service';
import { Menu } from './surt02.component';

export interface Surt02DetailData {
  menu: Menu | null;
  parentMenus: Menu[];
}

export const surt02Resolver: ResolveFn<Surt02DetailData> = (route, state) => {
  const id = route.paramMap.get('id');
  const service = inject(Surt02Service);
  const router = inject(Router);

  const parentMenus$ = service.getMenus(1, 1000).pipe(map((res: any) => res.items));
  const menu$ = (id && id !== 'new') ? service.getMenu(id) : of(null);

  return forkJoin({
    menu: menu$,
    parentMenus: parentMenus$
  }).pipe(
    catchError(() => {
      router.navigate(['/su/surt02']);
      return EMPTY;
    })
  );
};
