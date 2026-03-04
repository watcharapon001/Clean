import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedList } from '../surt01/surt01.service';

export interface User {
  userId: string;
  username: string;
  email: string;
  employeeId?: string;
  employeeName?: string;
  isActive: boolean;
  profileIds: string[];
  profileNames: string[];
  userOrgs: UserOrg[];
  password?: string;
}

export interface UserOrg {
  orgId: string;
  orgCode: string;
  orgName: string;
  isDefault: boolean;
}

export interface Employee {
  employeeId: string;
  firstName: string;
  lastName: string;
  employeeCode: string;
}

@Injectable({
  providedIn: 'root'
})
export class Surt04Service {
  private http = inject(HttpClient);
  private apiUrl = '/api/su/surt04';
  private employeeApiUrl = '/api/dbrt01';

  getUsers(
    pageNumber: number = 1,
    pageSize: number = 10,
    sortColumn?: string,
    sortDirection?: string
  ): Observable<PaginatedList<User>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (sortColumn) {
      params = params.set('sortColumn', sortColumn);
      if (sortDirection) {
        params = params.set('sortDirection', sortDirection);
      }
    }

    return this.http.get<PaginatedList<User>>(this.apiUrl, { params });
  }

  getUser(id: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  createUser(user: User): Observable<string> {
    return this.http.post<string>(this.apiUrl, user);
  }

  updateUser(id: string, user: User): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, user);
  }

  deleteUser(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getEmployees(pageNumber: number = 1, pageSize: number = 1000): Observable<PaginatedList<Employee>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<PaginatedList<Employee>>(this.employeeApiUrl, { params });
  }

  getOrganizes(): Observable<Organization[]> {
    return this.http.get<Organization[]>(`${this.apiUrl}/organizes`);
  
}

}

export interface Organization {
  orgId: string;
  orgCode: string;
  orgName: string;
}
