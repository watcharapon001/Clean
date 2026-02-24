import { Component, Input, Optional, Self } from '@angular/core';
import { ControlValueAccessor, NgControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'textbox',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule],
  templateUrl: './textbox.component.html',
  styleUrls: ['./textbox.component.scss']
})
export class TextboxComponent implements ControlValueAccessor {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() hint: string = '';
  @Input() type: string = 'text';
  @Input() maxLength: number | null = null;

  value: string = '';
  disabled: boolean = false;

  onChange: any = () => {};
  onTouched: any = () => {};

  constructor(@Optional() @Self() public ngControl: NgControl) {
    if (this.ngControl != null) {
      this.ngControl.valueAccessor = this;
    }
  }

  writeValue(value: any): void {
    this.value = value || '';
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onInput(event: Event) {
    const input = event.target as HTMLInputElement;
    this.value = input.value;
    this.onChange(this.value);
  }

  onBlur() {
    this.onTouched();
  }

  getErrorMessage(): string {
    if (!this.ngControl || !this.ngControl.errors) return '';
    const errors = this.ngControl.errors;
    if (errors['required']) return 'กรุณากรอกข้อมูล'; 
    if (errors['email']) return 'รูปแบบอีเมลไม่ถูกต้อง';
    if (errors['minlength']) return `ต้องมีอย่างน้อย ${errors['minlength'].requiredLength} ตัวอักษร`;
    if (errors['maxlength']) return `ไม่สามารถเกิน ${errors['maxlength'].requiredLength} ตัวอักษร`;
    return 'ข้อมูลไม่ถูกต้อง';
  }
}
