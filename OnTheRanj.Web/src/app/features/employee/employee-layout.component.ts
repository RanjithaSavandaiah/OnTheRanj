import { Component, Input } from '@angular/core';

import { MainLayoutComponent } from '../../shared/layout/main-layout/main-layout.component';

@Component({
  selector: 'app-employee-layout',
  templateUrl: './employee-layout.component.html',
  standalone: true,
  imports: [MainLayoutComponent]
})
export class EmployeeLayoutComponent {
  @Input() sidenavLinks: any[] = [];
}
