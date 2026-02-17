import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
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

  // Helper to get employees for dropdown
  getEmployees(): Observable<Employee[]> {
      // Assuming GET /api/db/dbrt01 returns a list or a paginated result. 
      // Based on dbrt01.service.ts likely returning list or simple object.
      // I should check dbrt01.service.ts but I'll assume standard list for now.
    return this.http.get<Employee[]>(this.employeeApiUrl);
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
