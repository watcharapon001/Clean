import { Component, Input, Optional, Self } from '@angular/core';
import { ControlValueAccessor, NgControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'number',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule],
  templateUrl: './number.component.html',
  styleUrls: ['./number.component.scss']
})
export class NumberComponent implements ControlValueAccessor {
  @Input() label: string = '';
  @Input() placeholder: string = '';
  @Input() hint: string = '';
  @Input() allowDecimals: boolean = false;
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
    this.value = value !== null && value !== undefined ? value.toString() : '';
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

  onKeyPress(event: KeyboardEvent) {
    const charCode = event.which ? event.which : event.keyCode;
    // Numbers (48-57)
    if (charCode >= 48 && charCode <= 57) {
      return true;
    }
    // Decimal point
    if (this.allowDecimals && charCode === 46) {
      if (this.value && this.value.includes('.')) {
        event.preventDefault();
        return false;
      }
      return true;
    }
    // Prevent default for other characters
    event.preventDefault();
    return false;
  }

  onInput(event: Event) {
    const input = event.target as HTMLInputElement;
    let val = input.value;
    
    // Fallback: Remove non-numeric characters via regex (e.g. paste)
    if (this.allowDecimals) {
      val = val.replace(/[^0-9.]/g, '');
      const parts = val.split('.');
      if (parts.length > 2) {
        val = parts[0] + '.' + parts.slice(1).join('');
      }
    } else {
      val = val.replace(/[^0-9]/g, '');
    }

    input.value = val;
    this.value = val;

    // Emit numeric value or null
    const numValue = val === '' ? null : Number(val);
    this.onChange(numValue);
  }

  onBlur() {
    this.onTouched();
  }

  getErrorMessage(): string {
    if (!this.ngControl || !this.ngControl.errors) return '';
    const errors = this.ngControl.errors;
    if (errors['required']) return 'กรุณากรอกข้อมูล'; 
    if (errors['min']) return 'ค่าน้อยที่สุดคือ ' + errors['min'].min;
    if (errors['max']) return 'ค่ามากที่สุดคือ ' + errors['max'].max;
    return 'ข้อมูลไม่ถูกต้อง';
  }
}
