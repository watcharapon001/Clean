import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss']
})
export class AppCardComponent {
  @Input() title: string = '';
  @Input() customClass: string = '';
  @Input() showFooter: boolean = false;
}
