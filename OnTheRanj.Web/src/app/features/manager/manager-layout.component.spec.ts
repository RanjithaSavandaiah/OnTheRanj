import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ManagerLayoutComponent } from './manager-layout.component';
import { MainLayoutComponent } from '../../shared/layout/main-layout/main-layout.component';
import { ToolbarComponent } from '../../shared/layout/toolbar/toolbar.component';
import { MatIconModule } from '@angular/material/icon';
import { RouterOutlet } from '@angular/router';
import { AsyncPipe, DatePipe, NgIf, NgFor, NgClass } from '@angular/common';
import { By } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

describe('ManagerLayoutComponent', () => {
  let fixture: ComponentFixture<ManagerLayoutComponent>;
  let component: ManagerLayoutComponent;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        ManagerLayoutComponent,
        MainLayoutComponent,
        ToolbarComponent,
        MatIconModule,
        RouterOutlet,
        AsyncPipe,
        DatePipe,
        NgIf,
        NgFor,
        NgClass
      ],
      providers: [
        { provide: ActivatedRoute, useValue: {} }
      ]
    }).compileComponents();
    fixture = TestBed.createComponent(ManagerLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render the main layout', () => {
    const mainLayout = fixture.debugElement.query(By.css('app-main-layout'));
    expect(mainLayout).toBeTruthy();
  });

  it('should pass correct sidenavLinks to main layout', () => {
    const mainLayout = fixture.debugElement.query(By.css('app-main-layout'));
    expect(mainLayout).toBeTruthy();
    const sidenavLinks = mainLayout.componentInstance.sidenavLinks;
    expect(sidenavLinks).toBeTruthy();
    expect(Array.isArray(sidenavLinks)).toBe(true);
    expect(sidenavLinks.length).toBe(5);
    expect(sidenavLinks[0].label).toBe('Dashboard');
    expect(sidenavLinks[4].route).toBe('/manager/reports');
  });

  it('should render brand and subtitle', () => {
    const brand = fixture.nativeElement.querySelector('[brand]');
    const subtitle = fixture.nativeElement.querySelector('[subtitle]');
    expect(brand?.textContent).toContain('OnTheRanj');
    expect(subtitle?.textContent).toContain('Manager Portal');
  });

  it('should contain a router outlet', () => {
    const outlet = fixture.debugElement.query(By.directive(RouterOutlet));
    expect(outlet).toBeTruthy();
  });
});
