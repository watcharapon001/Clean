import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Dbrt01Component } from './dbrt01.component';
import { Dbrt01Service, Dbrt01 } from './dbrt01.service';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { vi, describe, it, expect, beforeEach, Mock } from 'vitest';

describe('Dbrt01Component', () => {
  let component: Dbrt01Component;
  let fixture: ComponentFixture<Dbrt01Component>;
  let dbrt01ServiceSpy: { getDbrt01s: Mock; deleteDbrt01: Mock };

  const mockDbrt01s: Dbrt01[] = [
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
        getDbrt01s: vi.fn(),
        deleteDbrt01: vi.fn()
    };

    await TestBed.configureTestingModule({
      imports: [Dbrt01Component, RouterTestingModule],
      providers: [
        { provide: Dbrt01Service, useValue: spy }
      ]
    }).compileComponents();

    dbrt01ServiceSpy = TestBed.inject(Dbrt01Service) as unknown as { getDbrt01s: Mock; deleteDbrt01: Mock };
    // Setup default return value
    dbrt01ServiceSpy.getDbrt01s.mockReturnValue(of(mockDbrt01s));

    fixture = TestBed.createComponent(Dbrt01Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load employees on init', () => {
    expect(dbrt01ServiceSpy.getDbrt01s).toHaveBeenCalled();
    expect(component.dataSource.data).toEqual(mockDbrt01s);
  });

  it('should delete employee when confirmed', () => {
    const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);
    dbrt01ServiceSpy.deleteDbrt01.mockReturnValue(of((void 0)));
    
    // Reset calls from init
    dbrt01ServiceSpy.getDbrt01s.mockClear();

    component.deleteEmployee(mockDbrt01s[0]);

    expect(window.confirm).toHaveBeenCalled();
    expect(dbrt01ServiceSpy.deleteDbrt01).toHaveBeenCalledWith('1');
    // Should reload employees after delete
    expect(dbrt01ServiceSpy.getDbrt01s).toHaveBeenCalled();
    
    confirmSpy.mockRestore();
  });

  it('should NOT delete employee when cancelled', () => {
    const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(false);
    
    component.deleteEmployee(mockDbrt01s[0]);

    expect(window.confirm).toHaveBeenCalled();
    expect(dbrt01ServiceSpy.deleteDbrt01).not.toHaveBeenCalled();

    confirmSpy.mockRestore();
  });
});
