import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Dbrt01Service, Dbrt01 } from './dbrt01.service';
import { describe, it, expect, beforeEach, afterEach } from 'vitest';

describe('Dbrt01Service', () => {
  let service: Dbrt01Service;
  let httpMock: HttpTestingController;
  const apiUrl = '/api/dbrt01';

  const mockDbrt01s: Dbrt01[] = [
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
      providers: [Dbrt01Service]
    });
    service = TestBed.inject(Dbrt01Service);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve employees', () => {
    service.getDbrt01s().subscribe(employees => {
      expect(employees.length).toBe(2);
      expect(employees).toEqual(mockDbrt01s);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockDbrt01s);
  });

  it('should retrieve a single employee', () => {
    const mockDbrt01 = mockDbrt01s[0];
    const id = '1';

    service.getDbrt01(id).subscribe(employee => {
      expect(employee).toEqual(mockDbrt01);
    });

    const req = httpMock.expectOne(`${apiUrl}/${id}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockDbrt01);
  });

  it('should create an employee', () => {
    const newDbrt01: Partial<Dbrt01> = {
      firstName: 'New',
      lastName: 'User'
    };
    const responseId = '123';

    service.createDbrt01(newDbrt01).subscribe(id => {
      expect(id).toBe(responseId);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newDbrt01);
    req.flush(responseId);
  });

  it('should update an employee', () => {
    const id = '1';
    const updatedDbrt01: Partial<Dbrt01> = {
       firstName: 'Updated'
    };

    service.updateDbrt01(id, updatedDbrt01).subscribe(() => {
        // success
    });

    const req = httpMock.expectOne(`${apiUrl}/${id}`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedDbrt01);
    req.flush({});
  });

  it('should delete an employee', () => {
    const id = '1';

    service.deleteDbrt01(id).subscribe(() => {
        // success
    });

    const req = httpMock.expectOne(`${apiUrl}/${id}`);
    expect(req.request.method).toBe('DELETE');
    req.flush({});
  });
});
