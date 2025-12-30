import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UnauthorizedComponent } from './unauthorized.component';
import { By } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

describe('UnauthorizedComponent', () => {
  let component: UnauthorizedComponent;
  let fixture: ComponentFixture<UnauthorizedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UnauthorizedComponent],
      providers: [
        { provide: ActivatedRoute, useValue: {} }
      ]
    }).compileComponents();
    fixture = TestBed.createComponent(UnauthorizedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render a mat-card', () => {
    const card = fixture.debugElement.query(By.css('mat-card'));
    expect(card).toBeTruthy();
  });

  it('should have a button to return to login', () => {
    const button = fixture.debugElement.query(By.css('button'));
    expect(button).toBeTruthy();
    const btnText = button.nativeElement.textContent.toLowerCase();
    expect(btnText).toMatch(/login|return|logout/);
  });

  it('should have an empty sidenavLinks array', () => {
    expect(component.sidenavLinks).toEqual([]);
  });
});
