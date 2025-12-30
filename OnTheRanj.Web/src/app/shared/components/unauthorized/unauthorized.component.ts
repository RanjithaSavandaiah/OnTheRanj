import { Component } from '@angular/core';
import { CommonModule, AsyncPipe, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ToolbarComponent } from '../../layout/toolbar/toolbar.component';
import { MainLayoutComponent } from '../../layout/main-layout/main-layout.component';

/**
 * Unauthorized access component
 * Displayed when user tries to access restricted routes
 */
@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    ToolbarComponent,
    MainLayoutComponent
  ],
  templateUrl: './unauthorized.component.html',
  styleUrls: ['./unauthorized.component.scss']
})
export class UnauthorizedComponent {
  // Sidenav links for MainLayoutComponent (empty for unauthorized)
  sidenavLinks = [];
}
