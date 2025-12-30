import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-toolbar',
  standalone: true,
  imports: [CommonModule, MatToolbarModule, MatIconModule, MatButtonModule],
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent {
  @Input() userFullName: string | null = null;
  @Output() logout = new EventEmitter<void>();
}
