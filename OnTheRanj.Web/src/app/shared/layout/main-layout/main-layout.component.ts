import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, MatSidenavModule, MatListModule, RouterModule, MatIconModule],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss']
})
export class MainLayoutComponent {
  @Input() sidenavLinks: Array<{ label: string, icon: string, route: string }>= [];
}
