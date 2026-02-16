import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EmployeeListComponent } from './employee-list.component';
import { EmployeeService, Employee } from '../employee.service';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { vi, describe, it, expect, beforeEach, Mock } from 'vitest';

describe('EmployeeListComponent', () => {
  let component: EmployeeListComponent;
  let fixture: ComponentFixture<EmployeeListComponent>;
  let employeeServiceSpy: { getEmployees: Mock; deleteEmployee: Mock };

  const mockEmployees: Employee[] = [
    {
      employeeId: '1',
      orgId: 'org1',
      employeeCode: 'EMP001',
      firstName: 'John',
      lastName: 'Doe',
      isActive: true
    }
  ];

  beforeEach(async () => {
    // specific cast to any to allow spy object compatible with service
    const spy = {
        getEmployees: vi.fn(),
        deleteEmployee: vi.fn()
    };

    await TestBed.configureTestingModule({
      imports: [EmployeeListComponent, RouterTestingModule],
      providers: [
        { provide: EmployeeService, useValue: spy }
      ]
    }).compileComponents();

    employeeServiceSpy = TestBed.inject(EmployeeService) as unknown as { getEmployees: Mock; deleteEmployee: Mock };
    // Setup default return value
    employeeServiceSpy.getEmployees.mockReturnValue(of(mockEmployees));

    fixture = TestBed.createComponent(EmployeeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load employees on init', () => {
    expect(employeeServiceSpy.getEmployees).toHaveBeenCalled();
    expect(component.employees()).toEqual(mockEmployees);
  });

  it('should delete employee when confirmed', () => {
    const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);
    employeeServiceSpy.deleteEmployee.mockReturnValue(of((void 0)));
    
    // Reset calls from init
    employeeServiceSpy.getEmployees.mockClear();

    component.deleteEmployee('1');

    expect(window.confirm).toHaveBeenCalled();
    expect(employeeServiceSpy.deleteEmployee).toHaveBeenCalledWith('1');
    // Should reload employees after delete
    expect(employeeServiceSpy.getEmployees).toHaveBeenCalled();
    
    confirmSpy.mockRestore();
  });

  it('should NOT delete employee when cancelled', () => {
    const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(false);
    
    component.deleteEmployee('1');

    expect(window.confirm).toHaveBeenCalled();
    expect(employeeServiceSpy.deleteEmployee).not.toHaveBeenCalled();

    confirmSpy.mockRestore();
  });
});
