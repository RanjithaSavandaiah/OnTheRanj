import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MainLayoutComponent } from './main-layout.component';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { DebugElement } from '@angular/core';

describe('MainLayoutComponent', () => {
  let component: MainLayoutComponent;
  let fixture: ComponentFixture<MainLayoutComponent>;

  const sidenavLinks = [
    { label: 'Dashboard', icon: 'dashboard', route: '/dashboard' },
    { label: 'Projects', icon: 'work', route: '/projects' },
    { label: 'Timesheets', icon: 'schedule', route: '/timesheets' }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        MainLayoutComponent,
        RouterTestingModule,
        MatSidenavModule,
        MatListModule,
        MatIconModule,
        NoopAnimationsModule
      ]
    }).compileComponents();
    fixture = TestBed.createComponent(MainLayoutComponent);
    component = fixture.componentInstance;
    component.sidenavLinks = sidenavLinks;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render all sidenav links', () => {
    fixture.detectChanges();
    const linkEls = fixture.debugElement.queryAll(By.css('a[mat-list-item]'));
    expect(linkEls.length).toBe(sidenavLinks.length);
    sidenavLinks.forEach((link, idx) => {
      const el: DebugElement = linkEls[idx];
      expect(el.nativeElement.textContent).toContain(link.label);
      const icon = el.query(By.css('mat-icon'));
      expect(icon.nativeElement.textContent).toContain(link.icon);
    });
  });

});
