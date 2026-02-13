import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { EmployeeService, Employee } from '../employee.service';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './employee-list.component.html',
  styles: []
})
export class EmployeeListComponent implements OnInit {
  private employeeService = inject(EmployeeService);
  employees = signal<Employee[]>([]);

  ngOnInit() {
    this.loadEmployees();
  }

  loadEmployees() {
    this.employeeService.getEmployees().subscribe(data => {
      this.employees.set(data);
    });
  }

  deleteEmployee(id: string) {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.deleteEmployee(id).subscribe(() => {
        this.loadEmployees();
      });
    }
  }
}
