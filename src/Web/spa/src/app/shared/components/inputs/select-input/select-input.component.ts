import { Component, Input, Optional, Self, OnInit, OnDestroy, ViewChild, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { ControlValueAccessor, NgControl, ReactiveFormsModule, FormControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule, MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { Observable, Subscription, map, startWith } from 'rxjs';

@Component({
  selector: 'select-input',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatAutocompleteModule, MatInputModule, MatIconModule],
  templateUrl: './select-input.component.html',
  styleUrls: ['./select-input.component.scss'],
  host: {
    '[class.has-value]': 'value != null'
  }
})
export class SelectInputComponent implements ControlValueAccessor, OnInit, OnDestroy, OnChanges {
  @ViewChild('trigger', { read: MatAutocompleteTrigger }) autoCompleteTrigger!: MatAutocompleteTrigger;

  @Input() label: string = '';
  @Input() placeholder: string = 'Select an option';
  @Input() hint: string = '';
  @Input() options: any[] = [];
  @Input() valueField: string = 'value';
  @Input() labelField: string = 'label';
  @Input() multiple: boolean = false;
  @Input() showClearOption: boolean = true;

  value: any = null;
  disabled: boolean = false;
  isPanelOpen: boolean = false;

  searchControl = new FormControl('');
  filteredOptions$!: Observable<any[]>;
  private sub = new Subscription();

  onChange: any = () => {};
  onTouched: any = () => {};

  constructor(@Optional() @Self() public ngControl: NgControl) {
    if (this.ngControl != null) {
      this.ngControl.valueAccessor = this;
    }
  }

  ngOnInit() {
    this.filteredOptions$ = this.searchControl.valueChanges.pipe(
      startWith(''),
      map(value => this.filterData(value || ''))
    );
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['options']) {
      if (this.value != null) {
        this.writeValue(this.value);
      }
      // Re-trigger the filteredOptions$ observable to evaluate the new dataset
      this.searchControl.setValue(this.searchControl.value || '');
    }
  }

  displayFn(opt: any): string {
    if (!opt) return '';
    if (typeof opt !== 'object') {
       const found = this.options?.find(o => o[this.valueField] === opt);
       return found ? found[this.labelField] : opt; 
    }
    return opt[this.labelField] || '';
  }

  private filterData(search: any): any[] {
    if (!this.options) return [];
    const filterValue = (typeof search === 'string' ? search : '').toLowerCase();
    
    // Always return all options if input is empty or matches currently selected option object exactly
    if (!filterValue || (typeof search === 'object' && search !== null)) {
       return this.options.slice();
    }

    return this.options.filter(option => {
        const label = option[this.labelField];
        return label && label.toString().toLowerCase().includes(filterValue);
    });
  }

  openDropdown(event: Event) {
    event.stopPropagation();
    this.autoCompleteTrigger.openPanel();
  }

  onFocus() {
    // When focusing, force the panel open and trigger a new evaluation
    if (!this.autoCompleteTrigger.panelOpen) {
      this.autoCompleteTrigger.openPanel();
    }
  }

  onBlur() {
    this.onTouched();
    // Revert the text to the selected option object when clicking away without selecting
    this.searchControl.setValue(this.getSelectedOption(), { emitEvent: false });
  }

  getSelectedOption(): any {
     if (this.value == null || !this.options) return '';
     const selectedOption = this.options.find(opt => opt[this.valueField] === this.value);
     return selectedOption || '';
  }



  clearSelection(event: Event) {
    event.stopPropagation();
    this.value = null;
    this.onChange(null);
    this.searchControl.setValue(''); // Reset the text field to empty
  }

  onSelectionChange(optionVal: any) {
    if (optionVal === null) {
       this.clearSelection(new Event('clear'));
       return;
    }
    const val = optionVal[this.valueField];
    this.value = val;
    this.onChange(val);
    this.searchControl.setValue(optionVal, { emitEvent: false });
  }

  writeValue(value: any): void {
    this.value = value;
    const obj = this.getSelectedOption();
    this.searchControl.setValue(obj, { emitEvent: false });
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
    if (isDisabled) {
        this.searchControl.disable();
    } else {
        this.searchControl.enable();
    }
  }

  getErrorMessage(): string {
    if (!this.ngControl || !this.ngControl.errors) return '';
    const errors = this.ngControl.errors;
    if (errors['required']) return 'กรุณาเลือกข้อมูล'; 
    return 'ข้อมูลไม่ถูกต้อง';
  }
}
