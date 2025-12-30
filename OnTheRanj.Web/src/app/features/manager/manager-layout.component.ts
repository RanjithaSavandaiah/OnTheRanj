import { Component } from '@angular/core';
import { MainLayoutComponent } from '../../shared/layout/main-layout/main-layout.component';
import { MatIconModule } from '@angular/material/icon';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-manager-layout',
  standalone: true,
  imports: [
    MainLayoutComponent,
    MatIconModule,
    RouterOutlet
  ],
  templateUrl: './manager-layout.component.html'
})
export class ManagerLayoutComponent {}
