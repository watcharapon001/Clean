import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Employee {
  employeeId: string;
  orgId: string;
  employeeCode: string;
  firstName?: string;
  lastName?: string;
  displayName?: string;
  email?: string;
  phone?: string;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private http = inject(HttpClient);
  private apiUrl = '/api/employees'; // Assuming standard REST convention

  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  getEmployee(id: string): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  createEmployee(employee: Partial<Employee>): Observable<string> {
    return this.http.post(this.apiUrl, employee, { responseType: 'text' });
  }

  updateEmployee(id: string, employee: Partial<Employee>): Observable<void> {
    console.log(this.http);
    console.log(this.apiUrl);
    console.log(id);
    console.log(employee);
    
    return this.http.put<void>(`${this.apiUrl}/${id}`, employee);
  }

  deleteEmployee(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
