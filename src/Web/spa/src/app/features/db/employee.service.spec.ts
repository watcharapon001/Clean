import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { EmployeeService, Employee } from './employee.service';
import { describe, it, expect, beforeEach, afterEach } from 'vitest';

describe('EmployeeService', () => {
  let service: EmployeeService;
  let httpMock: HttpTestingController;
  const apiUrl = '/api/employees';

  const mockEmployees: Employee[] = [
    {
      employeeId: '1',
      orgId: 'org1',
      employeeCode: 'EMP001',
      firstName: 'John',
      lastName: 'Doe',
      isActive: true
    },
    {
      employeeId: '2',
      orgId: 'org1',
      employeeCode: 'EMP002',
      firstName: 'Jane',
      lastName: 'Doe',
      isActive: true
    }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [EmployeeService]
    });
    service = TestBed.inject(EmployeeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve employees', () => {
    service.getEmployees().subscribe(employees => {
      expect(employees.length).toBe(2);
      expect(employees).toEqual(mockEmployees);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockEmployees);
  });

  it('should retrieve a single employee', () => {
    const mockEmployee = mockEmployees[0];
    const id = '1';

    service.getEmployee(id).subscribe(employee => {
      expect(employee).toEqual(mockEmployee);
    });

    const req = httpMock.expectOne(`${apiUrl}/${id}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockEmployee);
  });

  it('should create an employee', () => {
    const newEmployee: Partial<Employee> = {
      firstName: 'New',
      lastName: 'User'
    };
    const responseId = '123';

    service.createEmployee(newEmployee).subscribe(id => {
      expect(id).toBe(responseId);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newEmployee);
    req.flush(responseId);
  });

  it('should update an employee', () => {
    const id = '1';
    const updatedEmployee: Partial<Employee> = {
       firstName: 'Updated'
    };

    service.updateEmployee(id, updatedEmployee).subscribe(() => {
        // success
    });

    const req = httpMock.expectOne(`${apiUrl}/${id}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedEmployee);
    req.flush({});
  });

  it('should delete an employee', () => {
    const id = '1';

    service.deleteEmployee(id).subscribe(() => {
        // success
    });

    const req = httpMock.expectOne(`${apiUrl}/${id}`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });
});
